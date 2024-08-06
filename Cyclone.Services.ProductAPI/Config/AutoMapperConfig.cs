using AutoMapper;
using Cyclone.Services.ProductAPI.DTOs;
using Cyclone.Services.ProductAPI.Models;

namespace Cyclone.Services.ProductAPI.Config
{
	public class AutoMapperConfig : Profile
	{
		public AutoMapperConfig()
		{
			CreateMap<Product, ProductDto>().ReverseMap();
		}
	}
}
