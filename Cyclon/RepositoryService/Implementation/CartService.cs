using Cyclone.DTOs;
using Cyclone.RepositoryService.Abstraction;
using Cyclone.Utilities;

namespace Cyclone.RepositoryService.Implementation
{
	public class CartService : ICartService
    {
		private readonly IBaseService _baseService;

		public CartService(IBaseService baseService)
		{
			_baseService = baseService;
		}




        public async Task<ResponseDto> ApplyCouponAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = SD.CartUrl + "/api/cart/applycoupon",
                ApiType = SD.ApiType.POST,
                Data = cartDto
            });
        }



        public async Task<ResponseDto> CartUpsertAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = SD.CartUrl + "/api/cart/cartupsert",
                ApiType = SD.ApiType.POST,
                Data =  cartDto
            });
        }




        public async Task<ResponseDto> GetCartAsync(string id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = SD.CartUrl + "/api/cart/" + id
            });
        }



        public async Task<ResponseDto> RemoveCartAsync(string cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = SD.CartUrl + "/api/cart/removecart",
                ApiType = SD.ApiType.POST,
                Data = cartDetailsId
            });
        }
    }
}
