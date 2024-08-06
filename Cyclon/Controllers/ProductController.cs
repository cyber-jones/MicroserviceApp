using Cyclone.DTOs;
using Cyclone.RepositoryService.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Cyclone.Controllers
{
	public class ProductController : Controller
	{
		private IProductService _productService;

        public ProductController(IProductService productService)
        {
			_productService = productService;	
        }





        public async Task<IActionResult> Index()
		{
			try
			{
				var responseDto = await _productService.GetAllAsync();

				if (responseDto.Success == true && responseDto.Data != null)
				{
					List<ProductDto> products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(responseDto.Data));
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





		public IActionResult Create()
		{
			return View();
		}




		[HttpPost]
		public async Task<IActionResult> Create(ProductDto productDto)
		{
			try
			{
				if (productDto != null && ModelState.IsValid)
				{
					var responseDto = await _productService.CreateAsync(productDto);

					if (responseDto.Success)
					{
						TempData["success"] = responseDto.Message;
						return RedirectToAction(nameof(Index));
					}
					
					TempData["error"] = responseDto.Message;
					ModelState.AddModelError("", "Error: Could not create coupon");
				}
			}
			catch (Exception ex)
			{
				TempData["error"] = ex.Message;
			}

			return View(productDto);
		}







		[HttpPost]
		public async Task<IActionResult> Delete(string id)
		{
			try
			{
				if (id != null)
				{
					var responseDto = await _productService.DeleteByIdAsync(id);
					if (responseDto.Success)
					{
						TempData["success"] = responseDto.Message;
					}

					TempData["error"] = responseDto.Message;
				}

				TempData["error"] = "Faild to perform action";
			}
			catch (Exception ex)
			{
				TempData["error"] = ex.Message;
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
