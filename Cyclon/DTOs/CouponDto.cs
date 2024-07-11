namespace Cyclone.DTOs
{
	public class CouponDto
	{
		public Guid CouponId { get; set; }
		public string CouponCode { get; set; }
		public double DiscountAmount { get; set; }
		public double MinAmount { get; set; }
	}
}
