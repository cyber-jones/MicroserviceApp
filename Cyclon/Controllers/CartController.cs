using Cyclone.DTOs;
using Cyclone.RepositoryService.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Cyclone.Controllers
{
    public class CartController : Controller
    {
        private ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }





        public async Task<IActionResult> Index()
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(JwtRegisteredClaimNames.Sub).Value;

                var responseDto = await _cartService.GetCart(userId);

                if (responseDto.Success == true && responseDto.Data != null)
                {
                    CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(responseDto.Data));
                    return View(cartDto);
                }

                TempData["error"] = responseDto.Message;
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            return Redirect("/Home/Index");
        }





        public async Task<IActionResult> RemoveCart(string Id)
        {
            try
            {

                if (!string.IsNullOrEmpty(Id))
                {
                    var responseDto = await _cartService.RemoveCart(Id);

                    if (responseDto.Success == true)
                    {
                        TempData["success"] = responseDto.Message;
                    }
                    else
                    {
                        TempData["error"] = responseDto.Message;
                    }
                }
                else
                {
                    TempData["error"] = "Failed to perform action";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }








        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            try
            {

                if (!ModelState.IsValid && cartDto != null)
                {
                    var responseDto = await _cartService.ApplyCoupon(cartDto);

                    if (responseDto.Success == true)
                    {
                        TempData["success"] = responseDto.Message;
                    }
                    else
                    {
                        TempData["error"] = responseDto.Message;
                    }
                }
                else
                {
                    TempData["error"] = "Failed to perform action";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
