using System.ComponentModel.DataAnnotations;

namespace Cyclone.DTOs
{
	public class ProductDto
	{
		public Guid ProductId { get; set; }
		[Required]
		[Display(Name = "Name")]
		public string Name { get; set; }
		[Required]
		[Range(0, 100)]
		[Display(Name = "Price")]
		public double Price { get; set; }
		[Required]
		[Display(Name = "Description")]
		public string Description { get; set; }
		[Required]
		[Display(Name = "Image Url")]
		public string ImageUrl { get; set; }
		[Required]
		[Display(Name = "Category Name")]
		public string CategoryName { get; set; }
	}
}
