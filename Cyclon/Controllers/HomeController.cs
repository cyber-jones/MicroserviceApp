using Cyclone.DTOs;
using Cyclone.Models;
using Cyclone.RepositoryService.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Cyclon.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IProductService _productService;

		public HomeController(ILogger<HomeController> logger, IProductService productService)
		{
			_logger = logger;
			_productService = productService;
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
