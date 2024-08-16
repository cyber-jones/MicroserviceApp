using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Cyclone.Services.ShoppingCartAPI.DTOs
{
	public class CartDto
	{
		public CartHeaderDto CartHeaderDto { get; set; }
		public IEnumerable<CartDetailsDto>? CartDetailsDto { get; set; }
	}
}
