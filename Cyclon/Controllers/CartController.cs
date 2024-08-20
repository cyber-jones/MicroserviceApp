using Cyclone.DTOs;
using Cyclone.RepositoryService.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Cyclone.Controllers
{
    public class CartController : Controller
    {
        private ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }



        public async Task<IActionResult> Index(string id)
        {
            try
            {
                var responseDto = await _cartService.GetCart(id);

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
