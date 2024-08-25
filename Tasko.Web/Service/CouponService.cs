﻿using Tasko.Web.Models;
using Tasko.Web.Service.IService;
using Tasko.Web.Utility;

namespace Tasko.Web.Service;

public class CouponService(IBaseService baseService) : ICouponService
{
    public async Task<ResponseDto?> CreateCouponsAsync(CouponDto couponDto)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = couponDto,
            Url = SD.CouponAPIBase + "/api/coupon"
        });
    }

    public async Task<ResponseDto?> DeleteCouponsAsync(int id)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.DELETE,
            Url = SD.CouponAPIBase + "/api/coupon/" + id
        });
    }

    public async Task<ResponseDto?> GetAllCouponsAsync()
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.CouponAPIBase + "/api/coupon"
        });
    }

    public async Task<ResponseDto?> GetCouponAsync(string couponCode)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.CouponAPIBase + "/api/coupon/GetByCode/" + couponCode
        });
    }

    public async Task<ResponseDto?> GetCouponByIdAsync(int id)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.CouponAPIBase + "/api/coupon/" + id
        });
    }

    public async Task<ResponseDto?> UpdateCouponsAsync(CouponDto couponDto)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.PUT,
            Data = couponDto,
            Url = SD.CouponAPIBase + "/api/coupon"
        });
    }
}
