using AutoMapper;
using Cyclone.Services.ProductAPI.Data;
using Cyclone.Services.ProductAPI.DTO;
using Cyclone.Services.ProductAPI.DTOs;
using Cyclone.Services.ProductAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cyclone.Services.ProductAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductAPIController : ControllerBase
	{
		private readonly ProductDbContext _context;
		private readonly IMapper _mapper;

		public ProductAPIController(ProductDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}



		[HttpGet]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<ResponseDto>>> GetAll()
		{
			var response = new ResponseDto();

			try
			{
				response.Data = await _context.Products.ToListAsync();
				return Ok(response);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
				return StatusCode(StatusCodes.Status500InternalServerError, response);
			}
		}




		[HttpGet(template: "{id:guid}", Name = "GetById")]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ResponseDto>> GetById(string id)
		{
			var response = new ResponseDto();

			try
			{
				if (!string.IsNullOrEmpty(id))
				{
					response.Data = await _context.Products.FindAsync(Guid.Parse(id));
					return Ok(response);
				}

				response.Success = false;
				response.Message = "Invalid Id";
				return StatusCode(StatusCodes.Status400BadRequest, response);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
				return StatusCode(StatusCodes.Status500InternalServerError, response);
			}
		}




		[HttpGet("{name}", Name = "GetByName")]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ResponseDto>> GetByCode(string name)
		{
			var response = new ResponseDto();

			try
			{
				if (!string.IsNullOrEmpty(name))
				{
					response.Data = await _context.Products.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
					return Ok(response);
				}

				response.Success = false;
				response.Message = "Invalid coupon code";
				return StatusCode(StatusCodes.Status400BadRequest, response);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
				return StatusCode(StatusCodes.Status500InternalServerError, response);
			}
		}





		[HttpPost]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ResponseDto>> Post([FromBody] ProductDto model)
		{
			var response = new ResponseDto();

			try
			{
				if (model != null && ModelState.IsValid)
				{
					var coupon = _mapper.Map<Product>(model);

					await _context.Products.AddAsync(coupon);
					await _context.SaveChangesAsync();

					response.Message = "Created Successfully";
					return Created(nameof(Post), response);
				}

				response.Success = false;
				response.Message = "Failed to perform action";
				return StatusCode(StatusCodes.Status400BadRequest, response);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
				return StatusCode(StatusCodes.Status500InternalServerError, response);
			}
		}





		[HttpPut]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status205ResetContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ResponseDto>> Put([FromBody] ProductDto model)
		{
			var response = new ResponseDto();

			try
			{
				if (model != null && ModelState.IsValid)
				{
					var couponDb = await _context.Products.AsNoTracking().FirstOrDefaultAsync(c => c.ProductId == model.ProductId);
					var product = _mapper.Map<Product>(model);

					_context.Products.Update(product);
					await _context.SaveChangesAsync();

					return StatusCode(StatusCodes.Status205ResetContent, response);
				}

				response.Success = false;
				response.Message = "Failed to perform action";
				return StatusCode(StatusCodes.Status400BadRequest, response);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
				return StatusCode(StatusCodes.Status500InternalServerError, response);
			}
		}





		[HttpDelete("{id:guid}", Name = "DeleteById")]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ResponseDto>> DeleteById(string id)
		{
			var response = new ResponseDto();

			try
			{
				var couponDb = await _context.Products.FindAsync(Guid.Parse(id));

				if (couponDb != null)
				{
					_context.Products.Remove(couponDb);
					await _context.SaveChangesAsync();

					response.Message = "Deleted Successfully";
					return StatusCode(StatusCodes.Status204NoContent, response);
				}

				response.Message = "Product not found";
				return NotFound(response);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
				return StatusCode(StatusCodes.Status500InternalServerError, response);
			}
		}

	}
}
