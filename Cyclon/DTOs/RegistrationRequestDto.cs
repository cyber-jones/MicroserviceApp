using System.ComponentModel.DataAnnotations;

namespace Cyclone.DTOs
{
	public class RegistrationRequestDto
	{
		[Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
		[Required]
		public string? Name { get; set; }
		[Required]
		public string? Role { get; set; }
		[Required]
        [Display(Name = "PhoneNumber")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
		[Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
	}
}
