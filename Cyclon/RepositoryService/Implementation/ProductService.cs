using Cyclone.DTOs;
using Cyclone.RepositoryService.Abstraction;
using Cyclone.Utilities;

namespace Cyclone.RepositoryService.Implementation
{
	public class ProductService : IProductService
	{
		private readonly IBaseService _baseService;

		public ProductService(IBaseService baseService)
		{
			_baseService = baseService;
		}




		public async Task<ResponseDto> DeleteByIdAsync(string id)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.ProductUrl + "/api/Product/" + id,
				ApiType = SD.ApiType.DELETE
			});
		}

		public async Task<ResponseDto> GetAllAsync()
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.ProductUrl + "/api/Product",
			});
		}

		public async Task<ResponseDto> GetByNameAsync(string name)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.ProductUrl + "/api/product/" + name,
			});
		}

		public async Task<ResponseDto> GetByIdAsync(string id)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.ProductUrl + "/api/product/" + id,
			});
		}

		public async Task<ResponseDto> CreateAsync(ProductDto product)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.ProductUrl + "/api/product",
				Data = product,
				ApiType = SD.ApiType.POST
			});
		}

		public async Task<ResponseDto> UpdateAsync(ProductDto product)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.ProductUrl + "/api/product",
				Data = product,
				ApiType = SD.ApiType.PUT
			});
		}
	}
}
