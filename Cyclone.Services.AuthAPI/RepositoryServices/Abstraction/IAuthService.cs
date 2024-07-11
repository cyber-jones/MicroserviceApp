using Cyclone.Services.AuthAPI.DTOs;

namespace Cyclone.Services.AuthAPI.RepositoryServices.Abstraction
{
	public interface IAuthService
	{
		Task<string> RegisterAsync(RegistrationRequestDto registrationRequestDto);
		Task<UserDto> LoginAsync(LoginRequestDto loginRequestDto);
	}
}
