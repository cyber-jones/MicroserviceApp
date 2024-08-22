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
					var products = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(responseDto.Data));
					return View(products);
				}

				TempData["error"] = responseDto.Message;
			}
			catch (Exception ex)
			{
				TempData["error"] = ex.Message;
			}

			return Redirect("Home/Index");
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





		public async Task<IActionResult> Edit(string id)
		{
			try
			{
				if (!string.IsNullOrEmpty(id))
				{
					var responseDto = await _productService.GetByIdAsync(id);

					if (responseDto.Success)
					{
						var product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Data));
						return View(product);
					}

					TempData["error"] = responseDto.Message;
				}
				else
				{
					TempData["error"] = "Faild to perform acton";
				}
			}
			catch (Exception ex)
			{
				TempData["error"] = ex.Message;
			}

			return RedirectToAction(nameof(Index));
		}





		[HttpPost]
		public async Task<IActionResult> Edit(ProductDto productDto, string id)
		{
			try
			{
				if (productDto != null && ModelState.IsValid && !string.IsNullOrEmpty(id))
				{
					var responseDto = await _productService.GetByIdAsync(id);

					if (responseDto.Success)
					{
						var responseDto2 = await _productService.UpdateAsync(productDto);

						if (responseDto2.Success)
						{
							TempData["success"] = responseDto2.Message;
							return RedirectToAction(nameof(Index));
						}

						TempData["error"] = responseDto2.Message;
						return View(productDto);
					}
					
					TempData["error"] = responseDto.Message;
					ModelState.AddModelError("", "Error: Could not update product");
				}
			}
			catch (Exception ex)
			{
				TempData["error"] = ex.Message;
			}

			return View(productDto);
		}








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
						return RedirectToAction(nameof(Index));
					}

					TempData["error"] = responseDto.Message;
				}
				else
				{
					TempData["error"] = "Faild to perform action";
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
