﻿using System.ComponentModel.DataAnnotations;

namespace Cyclone.Services.AuthAPI.DTOs
{
	public class RegistrationRequestDto
	{
		[Required]
		public string? Email { get; set; }
		[Required]
		public string? Name { get; set; }
		[Required]
		public string? Role { get; set; }
		[Required]
		public string? PhoneNumber { get; set; }
		[Required]
		public string? Password { get; set; }
	}
}
