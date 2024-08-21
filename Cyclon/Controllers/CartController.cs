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
    }
}
