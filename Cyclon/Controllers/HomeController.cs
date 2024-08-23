using Cyclone.DTOs;
using Cyclone.Models;
using Cyclone.RepositoryService.Abstraction;
using Cyclone.RepositoryService.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Cyclon.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IProductService _productService;
		private readonly ICartService _cartService;

		public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
		{
			_logger = logger;
			_productService = productService;
            _cartService = cartService;
		}






		public async Task<IActionResult> Index()
		{
            try
            {
                var responseDto = await _productService.GetAllAsync();

                if (responseDto.Success == true && responseDto.Data != null)
                {
                    IEnumerable<ProductDto> products = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(responseDto.Data));
                    return View(products);
                }

                TempData["error"] = responseDto.Message;
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }

			return View(null);
        }






        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var responseDto = await _productService.GetByIdAsync(id);

                if (responseDto.Success == true && responseDto.Data != null)
                {
                    ProductDto products = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Data));
                    return View(products);
                }

                TempData["error"] = responseDto.Message;
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }







        [HttpPost]
        public async Task<IActionResult> Details(ProductDto productDto)
        {
            try
            {

                if (!ModelState.IsValid && productDto != null)
                {

                    if (productDto.Count == 0)
                    {
                        TempData["warning"] = "Please input number of products to add to chart";
                        return RedirectToAction(nameof(Details));
                    }

                    CartHeaderDto cartHeaderDto = new()
                    {
                        UserId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub).Value
                    };

                    CartDetailsDto cartDetailsDto = new()
                    {
                        ProductId = productDto.ProductId,
                        Count = productDto.Count
                    };

                    List<CartDetailsDto> cartDetailsDtos = [ cartDetailsDto ];

                    CartDto cartDto = new()
                    {
                        CartHeaderDto = cartHeaderDto,
                        CartDetailsDto = cartDetailsDtos
                    };

                    var responseDto = await _cartService.CartUpsertAsync(cartDto);

                    if (responseDto.Success == true)
                    {
                        TempData["success"] = responseDto.Message;
                        return RedirectToAction(nameof(Details));
                    }

                    TempData["error"] = responseDto.Message;
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

            return RedirectToAction(nameof(Details));
        }






        public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
