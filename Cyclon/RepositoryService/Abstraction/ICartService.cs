using Cyclone.DTOs;

namespace Cyclone.RepositoryService.Abstraction
{
	public interface ICartService
	{
		Task<ResponseDto> GetCartAsync(string id);
		Task<ResponseDto> CartUpsertAsync(CartDto cartDto);
		Task<ResponseDto> ApplyCouponAsync(CartDto cartDto);
		Task<ResponseDto> RemoveCartAsync(string cartDetailsId);
	}
}
