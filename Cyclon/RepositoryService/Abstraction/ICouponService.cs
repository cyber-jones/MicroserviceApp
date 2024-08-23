using Cyclone.DTOs;

namespace Cyclone.RepositoryService.Abstraction
{
	public interface ICouponService
	{
		Task<ResponseDto> GetAllAsync();
		Task<ResponseDto> GetByIdAsync(string id);
		Task<ResponseDto> GetByCodeAsync(string code);
		Task<ResponseDto> CreateAsync(CouponDto coupon);
		Task<ResponseDto> UpdateAsync(CouponDto coupon);
		Task<ResponseDto> DeleteByIdAsync(string id);
		Task<ResponseDto> DeleteByCodeAsync(string code);
	}
}
