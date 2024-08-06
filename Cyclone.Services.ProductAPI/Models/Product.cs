using System.ComponentModel.DataAnnotations;

namespace Cyclone.Services.ProductAPI.Models
{
	public class Product
	{
		[Key]
		public Guid ProductId { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
		public string Description { get; set; }
		public string ImageUrl { get; set; }
		public string CategoryName { get; set; }
	}
}
