using System.ComponentModel.DataAnnotations;

namespace Cyclone.DTOs
{
	public class CartDetailsDto
	{
		[Required]
		public Guid CartDetailsId { get; set; }
		[Required]
		public Guid CartHeaderId { get; set; }
		public CartHeaderDto? CartHeaderDto { get; set; }
		[Required]
		public Guid ProductId { get; set; }
		public ProductDto? ProductDto { get; set; }
		[Required]
		public int Count { get; set; }
	}
}
