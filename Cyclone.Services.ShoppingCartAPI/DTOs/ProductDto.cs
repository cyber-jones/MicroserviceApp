using System.ComponentModel.DataAnnotations;

namespace Cyclone.Services.ShoppingCartAPI.DTOs
{
	public class ProductDto
	{
		public Guid ProductId { get; set; }
		[Required]
		public string? Name { get; set; }
		[Required]
		[Range(0, 100)]
		public double Price { get; set; }
		[Required]
		public string? Description { get; set; }
		[Required]
		public string? ImageUrl { get; set; }
		[Required]
		public string? CategoryName { get; set; }
	}
}
