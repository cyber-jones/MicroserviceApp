﻿using Cyclone.Services.ShoppingCartAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cyclone.Services.ShoppingCartAPI.DTOs
{
	public class CartDetailsDto
	{
		[Required]
		public Guid CartDetailsId { get; set; }
		[Required]
		public Guid CartHeaderId { get; set; }
		public CartHeaderDto? CartHeaderDto { get; set; }
		[Required]
		public Guid ProductId { get; set; }
		public ProductDto? ProductDto { get; set; }
		[Required]
		public int Count { get; set; }
	}
}
