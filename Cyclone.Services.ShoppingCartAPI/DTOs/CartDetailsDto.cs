using Cyclone.Services.ShoppingCartAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cyclone.Services.ShoppingCartAPI.DTOs
{
	public class CartDetailsDto
	{
		public Guid CartDetailsId { get; set; }
		public Guid CartHeaderId { get; set; }
		public CartHeaderDto? CartHeaderDto { get; set; }
		public Guid ProductId { get; set; }
		public ProductDto? ProductDto { get; set; }
		public int Count { get; set; }
	}
}
