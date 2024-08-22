using AutoMapper;
using Cyclone.Services.CouponAPI.Data;
using Cyclone.Services.CouponAPI.DTO;
using Cyclone.Services.CouponAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cyclone.Services.CouponAPI.Controllers
{
	[Route("api/coupon")]
	[Authorize]
	[ApiController]
	public class CouponAPIController : ControllerBase
	{
		private readonly CouponDBContext _context;
		private readonly IMapper _mapper;

        public CouponAPIController(CouponDBContext context, IMapper mapper)
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
				response.Data = await _context.Coupons.ToListAsync();
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
					response.Data = await _context.Coupons.FindAsync(Guid.Parse(id));
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




		[HttpGet("{code}", Name = "GetByCode")]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ResponseDto>> GetByCode(string code)
		{
			var response = new ResponseDto();

			try
			{
				if (!string.IsNullOrEmpty(code))
				{
					response.Data = await _context.Coupons.FirstOrDefaultAsync(c => c.CouponCode == code);
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
		public async Task<ActionResult<ResponseDto>> Post([FromBody] CouponDto model)
		{
			var response = new ResponseDto();

			try
			{
				if (model != null && ModelState.IsValid)
				{
					var coupon = _mapper.Map<Coupon>(model);

					await _context.Coupons.AddAsync(coupon);
					await _context.SaveChangesAsync();

					response.Message = "Created Successfully";
					return CreatedAtAction(nameof(GetById), new { id = coupon.CouponId }, response);
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
		public async Task<ActionResult<ResponseDto>> Put([FromBody] CouponDto model)
		{
			var response = new ResponseDto();

			try
			{
				if (model != null && ModelState.IsValid)
				{
					var couponDb = await _context.Coupons.AsNoTracking().FirstOrDefaultAsync(c => c.CouponId == model.CouponId);
					var coupon = _mapper.Map<Coupon>(model);

					_context.Coupons.Update(coupon);
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
				var couponDb = await _context.Coupons.FindAsync(Guid.Parse(id));

				if (couponDb != null)
				{
					_context.Coupons.Remove(couponDb);
					await _context.SaveChangesAsync();

					response.Message = "Deleted Successfully";
					return StatusCode(StatusCodes.Status204NoContent, response);
				}

				response.Message = "Invalid Coupon code";
				return NotFound(response);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
				return StatusCode(StatusCodes.Status500InternalServerError, response);
			}
		}





		[HttpDelete("{code}", Name = "DeleteByCode")]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ResponseDto>> DeleteByCode(string code)
		{
			var response = new ResponseDto();

			try
			{
				var couponDb = await _context.Coupons.FirstOrDefaultAsync(c => c.CouponCode == code);

				if (couponDb != null)
				{
					_context.Coupons.Remove(couponDb);
					await _context.SaveChangesAsync();

					response.Message = "Deleted Successfully";
					return StatusCode(StatusCodes.Status204NoContent, response);
				}

				response.Message = "Invalid Coupon code";
				return StatusCode(StatusCodes.Status404NotFound, response);
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
