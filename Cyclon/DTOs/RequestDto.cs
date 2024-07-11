using static Cyclone.Utilities.SD;

namespace Cyclone.DTOs
{
	public class RequestDto
	{
		public ApiType ApiType { get; set; } = ApiType.GET;
		public object? Data { get; set; }
		public required string Url { get; set; }
		public string? AccessToken { get; set; }
	}
}
