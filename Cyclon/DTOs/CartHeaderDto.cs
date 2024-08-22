using System.ComponentModel.DataAnnotations;

namespace Cyclone.DTOs
{
	public class CartHeaderDto
	{
		public Guid CartHeaderId { get; set; }
		[Required]
		public string? UserId { get; set; }
		public string? CouponCode { get; set; }
		public double Discount { get; set; }
		public double CartTotal { get; set; }
	}
}
