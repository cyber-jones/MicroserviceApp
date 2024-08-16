using AutoMapper;
using Cyclone.Services.ShoppingCartAPI.DTOs;
using Cyclone.Services.ShoppingCartAPI.Models;

namespace Cyclone.Services.ShoppingCartAPI.Config
{
	public class AutoMapperConfig : Profile
	{
		public AutoMapperConfig()
		{
			CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
			CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
		}
	}
}
