using Cyclone.Services.ShoppingCartAPI.DTOs;
using Cyclone.Services.ShoppingCartAPI.RepositoryServices.Abstraction;
using Newtonsoft.Json;

namespace Cyclone.Services.ShoppingCartAPI.RepositoryServices.Implementation
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }




        public async Task<ResponseDto> GetCoupon(string couponCode)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Coupon");
                var response = await client.GetAsync("/api/Coupon/" + couponCode);
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseDto>(content);

            }
            catch (Exception ex)
            {
                return new ResponseDto()
                {
                    Success = false,
                    Message =ex.Message,
                };
            }
        }
    }
}
