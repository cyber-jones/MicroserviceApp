using Cyclone.DTOs;
using Cyclone.RepositoryService.Abstraction;
using Cyclone.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Cyclone.Controllers
{
	public class AuthController : Controller
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
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

                        var loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto));

						return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("CustomError", "Invalid Username or Password");
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
    }
}
