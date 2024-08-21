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





        public async Task<ResponseDto> CartUpsert(CartDto product)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = SD.CartUrl + "/api/cart/",
                ApiType = SD.ApiType.POST,
                Data = product
            });
        }




        public async Task<ResponseDto> GetCart(string id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = SD.CartUrl + "/api/cart/" + id
            });
        }
    }
}
