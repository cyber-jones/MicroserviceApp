using Cyclone.DTOs;

namespace Cyclone.RepositoryService.Abstraction
{
	public interface IProductService
	{
		Task<ResponseDto> GetAllAsync();
		Task<ResponseDto> GetByIdAsync(string id);
		Task<ResponseDto> GetByName(string name);
		Task<ResponseDto> CreateAsync(ProductDto product);
		Task<ResponseDto> UpdateAsync(ProductDto product);
		Task<ResponseDto> DeleteByIdAsync(string id);
	}
}
