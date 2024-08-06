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
				Url = SD.ProductUrl + "/api/ProductApi/" + id,
				ApiType = SD.ApiType.DELETE
			});
		}

		public async Task<ResponseDto> GetAllAsync()
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.ProductUrl + "/api/ProductApi",
			});
		}

		public async Task<ResponseDto> GetByName(string name)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.ProductUrl + "/api/ProductApi/" + name,
			});
		}

		public async Task<ResponseDto> GetByIdAsync(string id)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.ProductUrl + "/api/ProductApi/" + id,
			});
		}

		public async Task<ResponseDto> CreateAsync(ProductDto product)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.ProductUrl + "/api/ProductApi",
				Data = product,
				ApiType = SD.ApiType.POST
			});
		}

		public async Task<ResponseDto> UpdateAsync(ProductDto product)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				Url = SD.ProductUrl + "/api/ProductApi",
				Data = product,
				ApiType = SD.ApiType.PUT
			});
		}
	}
}
