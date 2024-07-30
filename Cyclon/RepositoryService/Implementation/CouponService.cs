using Cyclone.DTOs;
using Cyclone.RepositoryService.Abstraction;
using Cyclone.Utilities;

namespace Cyclone.RepositoryService.Implementation
{
	public class CouponService : ICouponService
	{
		private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }




        public async Task<ResponseDto> CreateAsync(CouponDto coupon)
		{
			return await _baseService.SendAsync(new RequestDto
			{
				Url = SD.CouponApiUrl + "/api/CouponApi",
				ApiType = SD.ApiType.POST,
				Data = coupon
			});
		}

		public async Task<ResponseDto> DeleteByCodeAsync(string code)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.CouponApiUrl + "/api/CouponApi/" + code,
				ApiType = SD.ApiType.DELETE
			});
		}

		public async Task<ResponseDto> DeleteByIdAsync(string id)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.CouponApiUrl + "/api/CouponApi/" + id,
				ApiType = SD.ApiType.DELETE
			});
		}

		public async Task<ResponseDto> GetAllAsync()
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.CouponApiUrl + "/api/CouponApi"
			});
		}

		public async Task<ResponseDto> GetByCode(string code)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.CouponApiUrl + "/api/CouponApi/" + code
			});
		}

		public async Task<ResponseDto> GetByIdAsync(string id)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.CouponApiUrl + "/api/CouponApi/" + id
			});
		}

		public async Task<ResponseDto> UpdateAsync(CouponDto coupon)
		{
			return await _baseService.SendAsync(new RequestDto
			{
				Url = SD.CouponApiUrl + "/api/CouponApi",
				ApiType = SD.ApiType.PUT,
				Data = coupon
			});
		}
	}
}
