using Cyclone.DTOs;

namespace Cyclone.RepositoryService.Abstraction
{
	public interface IBaseService
	{
		Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBearer = true);
	}
}
