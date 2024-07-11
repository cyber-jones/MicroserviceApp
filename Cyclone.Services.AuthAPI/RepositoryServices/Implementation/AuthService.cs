using Cyclone.Services.AuthAPI.Data;
using Cyclone.Services.AuthAPI.DTOs;
using Cyclone.Services.AuthAPI.Models;
using Cyclone.Services.AuthAPI.RepositoryServices.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cyclone.Services.AuthAPI.RepositoryServices.Implementation
{
	public class AuthService : IAuthService
	{
		private readonly AuthDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(
				AuthDbContext context,
				UserManager<ApplicationUser> userManager,
				SignInManager<ApplicationUser> signInManager,
				RoleManager<IdentityRole> roleManager
			)
        {
			_context = context;
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
        }



        public async Task<UserDto?> LoginAsync(LoginRequestDto loginRequestDto)
		{
			try
			{
				var user = await _context.ApplicationUsers.FirstOrDefaultAsync(a => a.UserName == loginRequestDto.UserName);

				if (user != null)
				{
					var isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

					if (isValid)
					{
						return new UserDto()
						{
							Id = user.Id,
							Name = user.Name,
							UserName = user.UserName,
							Email = user.Email,
							PhoneNumber = user.PhoneNumber
						};
					}
				}

				return null;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<string> RegisterAsync(RegistrationRequestDto registrationRequestDto)
		{
			try
			{
				ApplicationUser applicationUser = new()
				{
					Name = registrationRequestDto.Name,
					UserName = registrationRequestDto.Email,
					Email = registrationRequestDto.Email,
					NormalizedEmail = registrationRequestDto.Email.ToUpper(),
					NormalizedUserName = registrationRequestDto.Email.ToUpper(),
					PhoneNumber = registrationRequestDto.PhoneNumber
				};

				var result = await _userManager.CreateAsync(applicationUser, registrationRequestDto.Password);

				if (result.Succeeded)
				{
					var user = await _context.ApplicationUsers.FirstAsync(a => a.UserName.Equals(registrationRequestDto.Email));

					if (user != null)
					{
						UserDto userDto = new()
						{
							UserName = user.UserName,
							Email = user.Email,
							PhoneNumber = user.PhoneNumber
						};
					}

					return "Success";
				}
				else
				{
					return result.Errors.ToString();
				}
			}
			catch (Exception ex) 
			{
				return ex.Message;
			}
		}
	}
}
