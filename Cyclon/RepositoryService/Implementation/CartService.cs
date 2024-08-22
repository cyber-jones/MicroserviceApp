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




        public async Task<ResponseDto> ApplyCoupon(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = SD.CartUrl + "/api/cart/applycoupon",
                ApiType = SD.ApiType.POST,
                Data = cartDto
            });
        }



        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = SD.CartUrl + "/api/cart/cartupsert",
                ApiType = SD.ApiType.POST,
                Data =  cartDto
            });
        }




        public async Task<ResponseDto> GetCart(string id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = SD.CartUrl + "/api/cart/" + id
            });
        }



        public async Task<ResponseDto> RemoveCart(string cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = SD.CartUrl + "/api/cart/removecart",
            });
        }
    }
}
