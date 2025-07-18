﻿using System.ComponentModel.DataAnnotations;

namespace Cyclone.Services.AuthAPI.DTOs
{
	public class UserDto
	{
		public string? Id { get; set; }
		public string? Email { get; set; }
		public string? Name { get; set; }
		public string? UserName { get; set; }
		public string? PhoneNumber { get; set; }
	}
}
