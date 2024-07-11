using Cyclone.DTOs;
using Cyclone.RepositoryService.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Cyclone.Controllers
{
	public class CouponController : Controller
	{
		private ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;	
        }




        public async Task<IActionResult> Index()
		{
			try
			{
				var responseDto = await _couponService.GetAllAsync();

				if (responseDto.Success == true && responseDto.Data != null)
				{
					List<CouponDto> coupons = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(responseDto.Data));
					return View(coupons);
				}
			}
			catch (Exception ex)
			{
				TempData["error"] = ex.Message;
			}

			return View(null);
		}




		public async Task<IActionResult> Create()
		{
			return View();
		}



		[HttpPost]
		public async Task<IActionResult> Create(CouponDto couponDto)
		{
			var responseDto = await _couponService.CreateAsync(couponDto);
			return View(responseDto);
		}
	}
}
