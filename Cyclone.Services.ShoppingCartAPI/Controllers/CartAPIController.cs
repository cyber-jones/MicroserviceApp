using AutoMapper;
using Cyclone.Services.ShoppingCartAPI.Data;
using Cyclone.Services.ShoppingCartAPI.DTOs;
using Cyclone.Services.ShoppingCartAPI.Models;
using Cyclone.Services.ShoppingCartAPI.RepositoryServices.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection.PortableExecutable;

namespace Cyclone.Services.ShoppingCartAPI.Controllers
{
	[Route("api/cart")]
	[Authorize]
	[ApiController]
	public class CartAPIController : ControllerBase
	{
		private readonly CartDbContext _context;
		private readonly IMapper _mapper;
		private readonly IProductService _productService;
		private readonly ICouponService _couponService;


        public CartAPIController(
			CartDbContext cartDbContext, 
			IMapper mapper, 
			IProductService productService, 
			ICouponService couponService
		)
        {
            _context = cartDbContext;
			_mapper = mapper;
			_productService = productService;
			_couponService = couponService;
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

					var responseProduct = await _productService.GetProducts();
					if (responseProduct == null || !responseProduct.Success)
						return StatusCode(StatusCodes.Status500InternalServerError, responseProduct);

					var products = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(responseProduct.Data));

					foreach (var items in cartDto.CartDetailsDto)
					{
						items.ProductDto = products.FirstOrDefault(p => p.ProductId == items.ProductId);
						cartDto.CartHeaderDto.CartTotal += (items.Count * items.ProductDto.Price);
					}

					if (!string.IsNullOrEmpty(cartDto.CartHeaderDto.CouponCode))
					{
                        var responseCoupon = await _couponService.GetCoupon(cartDto.CartHeaderDto.CouponCode);
                        if (responseCoupon == null || !responseCoupon.Success)
                            return StatusCode(StatusCodes.Status500InternalServerError, responseCoupon);

                        var coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseCoupon.Data));

                        if (cartDto.CartHeaderDto.CartTotal > coupon.MinAmount)
						{
							cartDto.CartHeaderDto.Discount = coupon.DiscountAmount;
							cartDto.CartHeaderDto.CartTotal -= coupon.DiscountAmount;
						}
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
				var cartDetailsFromBody = cartDto.CartDetailsDto.First();

				if (cartHeaderFromDb == null)
				{
					CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeaderDto);
					await _context.CartHeaders.AddAsync(cartHeader);
					await _context.SaveChangesAsync();

					CartDetails cartDetails = _mapper.Map<CartDetails>(cartDetailsFromBody);
					cartDetails.CartHeaderId = cartHeader.CartHeaderId;
					await _context.CartDetails.AddAsync(cartDetails);
					await _context.SaveChangesAsync();

					responseDto.Message = "New item added to cart";
					return CreatedAtAction(nameof(GetCart), new { userId = cartHeader.UserId }, responseDto);
				}
				else
				{
					var cartDetailsFromDb = await _context.CartDetails.AsNoTracking()
						.FirstOrDefaultAsync(u => u.ProductId == cartDto.CartDetailsDto.First().ProductId && u.CartHeaderId == cartHeaderFromDb.CartHeaderId);

					if (cartDetailsFromDb == null)
					{
						//create cartDetails
						cartDetailsFromBody.CartHeaderId = cartHeaderFromDb.CartHeaderId;
						await _context.CartDetails.AddAsync(_mapper.Map<CartDetails>(cartDetailsFromBody));
						await _context.SaveChangesAsync();
						responseDto.Message = "Item added to cart";
						return CreatedAtAction(nameof(GetCart), new { userId = cartHeaderFromDb.UserId }, responseDto);
					}
					else
					{
						//update count in cart details
						cartDetailsFromDb.Count += cartDetailsFromBody.Count;
						cartDetailsFromDb.CartHeaderId = cartHeaderFromDb.CartHeaderId;
						_context.CartDetails.Update(cartDetailsFromDb);
						await _context.SaveChangesAsync();
						responseDto.Message = "New item added to cart";
						return StatusCode(StatusCodes.Status205ResetContent, responseDto);
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











        [HttpPost("ApplyCoupon")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status205ResetContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseDto>> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                ResponseDto responseDto = new();

				if (cartDto != null)
				{
                    CartHeader cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == cartDto.CartHeaderDto.UserId);

                    if (cartHeader != null)
                    {
                        cartHeader.CouponCode = cartDto.CartHeaderDto.CouponCode;
                        await _context.SaveChangesAsync();

                        responseDto.Message = "Coupon Added";
                        return Ok(responseDto);
                    }
                }

                responseDto.Message = "Failded to apply coupon";
				responseDto.Success = false;
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
    }
}
