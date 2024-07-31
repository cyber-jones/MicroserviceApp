using System.ComponentModel.DataAnnotations;

namespace Cyclone.DTOs
{
	public class LoginRequestDto
	{
		[Required]
        [Display(Name = "Username")]
        public string? UserName { get; set; }
		[Required]
		[Display( Name = "Password")]
		[DataType(DataType.Password)]
		public string? Password { get; set; }
	}
}
