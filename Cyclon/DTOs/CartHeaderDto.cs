using System.ComponentModel.DataAnnotations;

namespace Cyclone.DTOs
{
	public class CartHeaderDto
	{
		public Guid CartHeaderId { get; set; }
		[Required]
		public string? UserId { get; set; }
		[Required]
		public string? CouponCode { get; set; }
		[Required]
		public double Discount { get; set; }
		[Required]
		public double CartTotal { get; set; }
	}
}
