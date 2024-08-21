using Cyclone.Services.ShoppingCartAPI.DTOs;

namespace Cyclone.Services.ShoppingCartAPI.RepositoryServices.Abstraction
{
    public interface ICouponService
    {
        Task<ResponseDto> GetCoupon(string couponCode);
    }
}
