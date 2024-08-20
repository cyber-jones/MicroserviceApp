using AutoMapper;
using Cyclone.Services.ShoppingCartAPI.Data;
using Cyclone.Services.ShoppingCartAPI.DTO;
using Cyclone.Services.ShoppingCartAPI.DTOs;
using Cyclone.Services.ShoppingCartAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace Cyclone.Services.ShoppingCartAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartAPIController : ControllerBase
	{
		private readonly CartDbContext _context;
		private readonly IMapper _mapper;
        public CartAPIController(CartDbContext cartDbContext, IMapper mapper)
        {
            _context = cartDbContext;
			_mapper = mapper;
        }



        [HttpGet("{userId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ResponseDto>> GetCart(string userId)
		{
			try
			{
				ResponseDto responseDto = new();
				var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId);

				if (cartHeader != null && !string.IsNullOrEmpty(userId))
				{
					var cartsDetails =  _context.CartDetails.Where(u => u.CartHeaderId == cartHeader.CartHeaderId);

					CartDto cartDto = new()
					{
						CartHeaderDto = _mapper.Map<CartHeaderDto>(cartHeader),
						CartDetailsDto = _mapper.Map<IEnumerable<CartDetailsDto>>(cartsDetails)
					};

					foreach (var items in cartDto.CartDetailsDto)
					{
						cartDto.CartHeaderDto.CartTotal += (items.Count * items.ProductDto.Price);
					}

					responseDto.Data = cartDto;

					return Ok(responseDto);
				}

				responseDto.Success = false;
				responseDto.Message = "Empty cart";
				return BadRequest(responseDto);
				
			}
			catch (Exception ex)
			{
				ResponseDto responseDto = new()
				{
					Success = false,
					Message = ex.Message
				};

				return StatusCode(StatusCodes.Status500InternalServerError, responseDto);
			}
		}







		[HttpPost("CartUpsert")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status205ResetContent)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<ResponseDto>> CartUpsert([FromBody] CartDto cartDto)
		{
			try
			{
				ResponseDto responseDto = new();
				var cartHeaderFromDb = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeaderDto.UserId);

				if (cartHeaderFromDb == null)
				{
					CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeaderDto);
					await _context.CartHeaders.AddAsync(cartHeader);
					await _context.SaveChangesAsync();

					CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetailsDto.First());
					cartDetails.CartHeaderId = cartHeader.CartHeaderId;
					await _context.CartDetails.AddAsync(cartDetails);
					await _context.SaveChangesAsync();

					responseDto.Message = "New item added to cart";
					return CreatedAtAction(nameof(GetCart), new { id = cartHeader.UserId }, responseDto);
				}
				else
				{
					var cartDetailsFromDb = await _context.CartDetails.AsNoTracking()
						.FirstOrDefaultAsync(u => u.ProductId == cartDto.CartDetailsDto.First().ProductId && u.CartHeaderId == cartHeaderFromDb.CartHeaderId);

					if (cartDetailsFromDb == null)
					{
						//create cartDetails
						cartDto.CartDetailsDto.First().CartDetailsId = cartHeaderFromDb.CartHeaderId;
						await _context.CartDetails.AddAsync(_mapper.Map<CartDetails>(cartDto.CartDetailsDto.First()));
						await _context.SaveChangesAsync();
						responseDto.Message = "Item added to cart";
						return CreatedAtAction(nameof(GetCart), new { id = cartHeaderFromDb.UserId }, responseDto);
					}
					else
					{
						//update count in cart details
						cartDto.CartDetailsDto.First().Count += cartDetailsFromDb.Count;
						cartDto.CartDetailsDto.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
						cartDto.CartDetailsDto.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
						await _context.SaveChangesAsync();
						responseDto.Message = "New item added to cart";
						return StatusCode(StatusCodes.Status205ResetContent);
					}
				}
			}
			catch (Exception ex)
			{
				ResponseDto responseDto = new()
				{
					Success = false,
					Message = ex.Message
				};

				return StatusCode(StatusCodes.Status500InternalServerError, responseDto);
			}
		}











        [HttpPost("RemoveCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseDto>> RemoveCart([FromBody] string cartDetailsId)
        {
            try
            {
                ResponseDto responseDto = new();
                CartDetails cartDetails = await _context.CartDetails.FirstOrDefaultAsync(c => c.CartDetailsId == Guid.Parse(cartDetailsId));

                int cartCount = await _context.CartDetails.CountAsync();
                _context.CartDetails.Remove(cartDetails);

                if (cartCount == 1)
                {
                    CartHeader cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.CartHeaderId == cartDetails.CartHeaderId);
                    _context.CartHeaders.Remove(cartHeader);
                }

                await _context.SaveChangesAsync();
                responseDto.Message = "Produc removed from cart";

                return StatusCode(StatusCodes.Status204NoContent, responseDto);
            }
            catch (Exception ex)
            {
                ResponseDto responseDto = new()
                {
                    Success = false,
                    Message = ex.Message
                };

                return StatusCode(StatusCodes.Status500InternalServerError, responseDto);
            }
        }
    }
}
