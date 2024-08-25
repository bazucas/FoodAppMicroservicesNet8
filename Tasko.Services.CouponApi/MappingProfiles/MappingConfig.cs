using AutoMapper;
using Tasko.Services.CouponApi.Dto;
using Tasko.Services.CouponApi.Models;

namespace Tasko.Services.CouponApi.MappingProfiles;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            _ = config.CreateMap<CouponDto, Coupon>();
            _ = config.CreateMap<Coupon, CouponDto>();
        });
        return mappingConfig;
    }
}
