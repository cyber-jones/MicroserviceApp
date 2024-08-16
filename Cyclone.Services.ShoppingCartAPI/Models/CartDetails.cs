using Cyclone.Services.ShoppingCartAPI.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cyclone.Services.ShoppingCartAPI.Models
{
	public class CartDetails
	{
		[Key]
		public Guid CartDetailsId { get; set; }
		public Guid CartHeaderId { get; set; }
		[ForeignKey(nameof(CartHeaderId))]
		public CartHeader? CartHeader { get; set; }
		public Guid ProductId { get; set; }
		public int Count { get; set; }
		[NotMapped]
		public ProductDto? ProductDto { get; set; }
	}
}
