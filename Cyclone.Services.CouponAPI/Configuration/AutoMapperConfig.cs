using AutoMapper;
using Cyclone.Services.CouponAPI.DTO;
using Cyclone.Services.CouponAPI.Models;

namespace Cyclone.Services.CouponAPI.Configuration
{
	public class AutoMapperConfig : Profile
	{
        public AutoMapperConfig()
        {
            CreateMap<Coupon, CouponDto>().ReverseMap();
        }


        /*
		 * Another method of mapper configuration via dependency injection package
		 */
        //public static  MapperConfiguration RegisterMap() 
        //{
        //	var mapper = new MapperConfiguration(config =>
        //		config.CreateMap<Coupon, CouponDto>().ReverseMap()
        //	);

        //	return mapper;
        //}
    }
}

