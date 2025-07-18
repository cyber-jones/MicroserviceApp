﻿using Cyclone.DTOs;
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
				Url = SD.CouponApiUrl + "/api/Coupon",
				ApiType = SD.ApiType.POST,
				Data = coupon
			});
		}

		public async Task<ResponseDto> DeleteByCodeAsync(string code)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.CouponApiUrl + "/api/Coupon/" + code,
				ApiType = SD.ApiType.DELETE
			});
		}

		public async Task<ResponseDto> DeleteByIdAsync(string id)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.CouponApiUrl + "/api/Coupon/" + id,
				ApiType = SD.ApiType.DELETE
			});
		}

		public async Task<ResponseDto> GetAllAsync()
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.CouponApiUrl + "/api/coupon"
			});
		}

		public async Task<ResponseDto> GetByCodeAsync(string code)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.CouponApiUrl + "/api/coupon/" + code
			});
		}

		public async Task<ResponseDto> GetByIdAsync(string id)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.CouponApiUrl + "/api/coupon/" + id
			});
		}

		public async Task<ResponseDto> UpdateAsync(CouponDto coupon)
		{
			return await _baseService.SendAsync(new RequestDto
			{
				Url = SD.CouponApiUrl + "/api/coupon",
				ApiType = SD.ApiType.PUT,
				Data = coupon
			});
		}
	}
}
