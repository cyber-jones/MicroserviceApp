using Cyclone.DTOs;

namespace Cyclone.RepositoryService.Abstraction
{
	public interface ICartService
	{
		Task<ResponseDto> GetCart(string id);
		Task<ResponseDto> CartUpsert(CartDto product);
	}
}
