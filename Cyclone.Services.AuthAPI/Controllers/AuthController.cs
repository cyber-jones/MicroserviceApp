using Cyclone.Services.AuthAPI.Data;
using Cyclone.Services.AuthAPI.DTOs;
using Cyclone.Services.AuthAPI.RepositoryServices.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cyclone.Services.AuthAPI.Controllers
{
	[Route("api/auth")]
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





		[HttpPost("login")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ResponseDto>> Login([FromBody] LoginRequestDto loginRequestDto)
		{
            try
			{
                ResponseDto responseDto = new();

                if (loginRequestDto != null && ModelState.IsValid)
				{
					var loginResponseDto = await _authService.LoginAsync(loginRequestDto);

					if (loginResponseDto.User != null)
					{
						//Generate Token 
						var token = _tokenGenerator.GenerateToken(loginResponseDto.User);

						if (token != null)
						{
                            loginResponseDto.Token = token;
                            responseDto.Data = loginResponseDto;
                            return Ok(responseDto);
                        }
					}
					else
					{
						responseDto.Success = false;
						responseDto.Message = loginResponseDto.Message;
						return StatusCode(StatusCodes.Status401Unauthorized, responseDto);
					}
				}

				responseDto.Message = "Invalid Username or Password";
				responseDto.Success = false;
				return BadRequest(responseDto);
			}
			catch (Exception ex)
			{
				ResponseDto responseDto = new()
				{
					Success = false,
					Message = ex.Message,
				};

                return StatusCode(StatusCodes.Status500InternalServerError, responseDto);
			}
		}








		[HttpPost("signup")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ResponseDto>> Register([FromBody] RegistrationRequestDto registrationRequestDto)
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









        [HttpPost("assignrole")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ResponseDto>> AssignRole([FromBody] RegistrationRequestDto registrationRequestDto)
		{
			try
			{
				if (registrationRequestDto.Email != null && registrationRequestDto.Role != null)
				{
					var success = await _authService.AssignRoleAsync(registrationRequestDto.Email, registrationRequestDto.Role);

					if (success)
					{
						_responseDto.Message = "Success";
						return Ok(_responseDto);
					}

					_responseDto.Success = false;
					_responseDto.Message = "Failed to assign role";
					return StatusCode(StatusCodes.Status500InternalServerError, _responseDto);
				}

				_responseDto.Success = false;
				_responseDto.Message = "Email and role required!";
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
