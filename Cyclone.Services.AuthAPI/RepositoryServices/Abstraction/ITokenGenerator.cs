using Cyclone.Services.AuthAPI.DTOs;

namespace Cyclone.Services.AuthAPI.RepositoryServices.Abstraction
{
	public interface ITokenGenerator
	{
		string GenerateToken(UserDto user);
	}
}
