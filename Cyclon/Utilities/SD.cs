namespace Cyclone.Utilities
{
	public class SD
	{
		public static string? CouponApiUrl { get; set; }
		public static string? AuthApiUrl { get; set; }
		public static string? Admin { get; set; } = "ADMIN";
		public static string? Employee { get; set; } = "EMPLOYEE";
		public static string? Company { get; set; } = "COMPANY";
		public static string? Customer { get; set; } = "CUSTOMER";
		public static string? Cookie { get; set; } = "JWTCookie";
		public enum ApiType
		{
			GET,
			POST, 
			PUT,
			DELETE
		}
	}
}
