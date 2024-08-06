using System.ComponentModel.DataAnnotations;

namespace Cyclone.DTOs
{
	public class CouponDto
	{
		public Guid CouponId { get; set; }
		[Required]
		[Display(Name = "Coupon Code")]
		public string CouponCode { get; set; }
		[Required]
		[Display(Name = "Discount Amount")]
		public double DiscountAmount { get; set; }
		[Required]
		[Display(Name = "Minimum Amount")]
		public double MinAmount { get; set; }
	}
}
