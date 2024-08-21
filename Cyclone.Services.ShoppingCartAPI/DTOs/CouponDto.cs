using System.ComponentModel.DataAnnotations;

namespace Cyclone.Services.ShoppingCartAPI.DTOs
{
	public class CouponDto
	{
		public Guid CouponId { get; set; }
		[Required]
		public string CouponCode { get; set; }
		[Required]
		public double DiscountAmount { get; set; }
		[Required]
		public double MinAmount { get; set; }
	}
}
