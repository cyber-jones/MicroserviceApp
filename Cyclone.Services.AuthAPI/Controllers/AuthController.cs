using Cyclone.Services.AuthAPI.Data;
using Cyclone.Services.AuthAPI.DTOs;
using Cyclone.Services.AuthAPI.RepositoryServices.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cyclone.Services.AuthAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly ITokenGenerator _tokenGenerator;
		private readonly ResponseDto _responseDto;


        public AuthController(IAuthService authService, ITokenGenerator tokenGenerator)
        {
			_authService = authService;
			_tokenGenerator = tokenGenerator;
			_responseDto = new();
        }




		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
		{
			try
			{
				LoginResponseDto loginResponseDto = new();

				if (loginRequestDto == null || ModelState.IsValid)
				{
					var user = await _authService.LoginAsync(loginRequestDto);

					if (user != null)
					{
						//Generate Token 
						var token = _tokenGenerator.GenerateToken(user);

						loginResponseDto.User = user;
						loginResponseDto.Token = token;

						return Ok(loginResponseDto);
					}
					else
					{
						return StatusCode(StatusCodes.Status401Unauthorized, loginResponseDto);
					}
				}
				return BadRequest(loginResponseDto);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}









		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
		{
			try
			{
				if (registrationRequestDto == null || ModelState.IsValid)
				{
					var message = await _authService.RegisterAsync(registrationRequestDto);

					if (message == "Success")
					{
						_responseDto.Message = message;
						return StatusCode(StatusCodes.Status201Created, _responseDto);
					}
					else
					{
						_responseDto.Success = false;
						_responseDto.Message = message;
						return StatusCode(StatusCodes.Status500InternalServerError, _responseDto);
					}
				}

				_responseDto.Success = false;
				_responseDto.Message = "An error has occurred";
				return BadRequest(_responseDto);
			}
			catch (Exception ex)
			{
				_responseDto.Success = false;
				_responseDto.Message = ex.Message;
				return StatusCode(StatusCodes.Status500InternalServerError, _responseDto);
			}
		}
	}
}
