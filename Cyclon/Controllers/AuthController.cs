using Cyclone.DTOs;
using Cyclone.RepositoryService.Abstraction;
using Cyclone.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Cyclone.Controllers
{
	public class AuthController : Controller
	{
		private readonly IAuthService _authService;
		private readonly ITokenProvider _tokenProvider;

		public AuthController(IAuthService authService, ITokenProvider tokenProvider)
		{
			_authService = authService;
            _tokenProvider = tokenProvider;
		}


		public IActionResult Login()
		{
			return View(); 
		}





		[HttpPost]
		public async Task<IActionResult> Login(LoginRequestDto model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var responseDto = await _authService.LoginAsync(model);

					if (responseDto.Success)
					{
						TempData["success"] = responseDto.Message;

                        var loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Data));

                        if (loginResponseDto.Token != null)
                        {
                            await SigninAsync(loginResponseDto.Token);
                            _tokenProvider.SetToken(loginResponseDto.Token);

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", loginResponseDto.Message);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", responseDto.Message);
                    }
				}

				return View(model);
			}
			catch (Exception ex)
			{
				TempData["error"] = ex.Message;
				return RedirectToAction(nameof(Login));
			}
		}





		public IActionResult SignUp()
		{
			IEnumerable<SelectListItem> Roles = [
				new SelectListItem(){
					Text = SD.Admin,
					Value = SD.Admin,
				},

				new SelectListItem(){
					Text = SD.Employee,
					Value = SD.Employee,
				},

				new SelectListItem(){
					Text = SD.Company,
					Value = SD.Company,
				},

				new SelectListItem(){
					Text = SD.Customer,
					Value = SD.Customer,
				},
			];

			ViewBag.Roles = Roles;
			return View();
		}






        [HttpPost]
        public async Task<IActionResult> Signup(RegistrationRequestDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var responseDto = await _authService.RegisterAsync(model);

                    if (responseDto.Success)
                    {

                        if (model.Role == null)
                        {
                            model.Role = SD.Customer;
                        }

                        var assignRole = await _authService.AssignRoleAsync(model);

                        if (assignRole.Success)
                        {
                            TempData["success"] = assignRole.Message;
                        }
                        else
                        {
                            TempData["error"] = assignRole.Message;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", responseDto.Message);
                    }
                }
                else
                {
                    IEnumerable<SelectListItem> Roles = [
                        new SelectListItem(){
                            Text = SD.Admin,
                            Value = SD.Admin,
                        },

                        new SelectListItem(){
                            Text = SD.Employee,
                            Value = SD.Employee,
                        },

                        new SelectListItem(){
                            Text = SD.Company,
                            Value = SD.Company,
                        },

                        new SelectListItem(){
                            Text = SD.Customer,
                            Value = SD.Customer,
                        }
                    ];

                    ViewBag.Roles = Roles;
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            return RedirectToAction(nameof(Login));
        }







        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
                _tokenProvider.ClearToken();
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            return RedirectToAction("Index", "Home");
        }











        private async Task SigninAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.ReadJwtToken(token);

            IEnumerable<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(n => n.Type == JwtRegisteredClaimNames.Name).Value),
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(s => s.Type == JwtRegisteredClaimNames.Sub).Value),
                new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(e => e.Type == JwtRegisteredClaimNames.Email).Value),
                new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(n => n.Type == JwtRegisteredClaimNames.Email).Value)
            ];

            var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claimsIdentity.AddClaims(claims);

            var claimPrinciple = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrinciple);
        }
    }
}
