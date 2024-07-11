namespace Cyclone.Services.AuthAPI.DTOs
{
	public class ResponseDto
	{
		public bool Success { get; set; } = true;
		public object? Data { get; set; }
		public string? Message { get; set; }
	}
}
