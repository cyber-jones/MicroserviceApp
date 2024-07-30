using Cyclone.DTOs;
using Cyclone.RepositoryService.Abstraction;
using Cyclone.Utilities;

namespace Cyclone.RepositoryService.Implementation
{
	public class AuthService : IAuthService
	{
		private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }



        public Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
		{
			return _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.POST,
				Url = SD.AuthApiUrl + "/login",
				Data = loginRequestDto
			});
		}


		public Task<ResponseDto> RegisterAsync(RegistrationRequestDto registrationRequestDto)
		{
			return _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.POST,
				Url = SD.AuthApiUrl + "/signup",
				Data = registrationRequestDto
			});
		}




		public Task<ResponseDto> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
		{
			return _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.POST,
				Url = SD.AuthApiUrl + "/assignrole",
				Data = registrationRequestDto
			});
		}
	}
}
