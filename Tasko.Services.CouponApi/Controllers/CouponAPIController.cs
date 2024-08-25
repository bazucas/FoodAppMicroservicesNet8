using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tasko.Services.CouponApi.Data;
using Tasko.Services.CouponApi.Dto;
using Tasko.Services.CouponApi.Models;


namespace Tasko.Services.CouponApi.Controllers;

[Route("api/coupon")]
[ApiController]
public class CouponApiController(AppDbContext db, IMapper mapper) : ControllerBase
{
    private readonly ResponseDto _response = new();

    [HttpGet]
    public ResponseDto Get()
    {
        try
        {
            IEnumerable<Coupon> objList = db.Coupons.ToList();
            _response.Result = mapper.Map<IEnumerable<CouponDto>>(objList);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }

    [HttpGet]
    [Route("{id:int}")]
    public ResponseDto Get(int id)
    {
        try
        {
            Coupon obj = db.Coupons.First(u => u.CouponId == id);
            _response.Result = mapper.Map<CouponDto>(obj);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }

    [HttpGet]
    [Route("GetByCode/{code}")]
    public ResponseDto GetByCode(string code)
    {
        try
        {
            Coupon obj = db.Coupons.First(u => u.CouponCode.ToLower() == code.ToLower());
            _response.Result = mapper.Map<CouponDto>(obj);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }

    [HttpPost]
    public ResponseDto Post([FromBody] CouponDto couponDto)
    {
        try
        {
            Coupon obj = mapper.Map<Coupon>(couponDto);
            _ = db.Coupons.Add(obj);
            _ = db.SaveChanges();

            _response.Result = mapper.Map<CouponDto>(obj);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }


    [HttpPut]
    public ResponseDto Put([FromBody] CouponDto couponDto)
    {
        try
        {
            Coupon obj = mapper.Map<Coupon>(couponDto);
            _ = db.Coupons.Update(obj);
            _ = db.SaveChanges();

            _response.Result = mapper.Map<CouponDto>(obj);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }

    [HttpDelete]
    [Route("{id:int}")]
    public ResponseDto Delete(int id)
    {
        try
        {
            Coupon obj = db.Coupons.First(u => u.CouponId == id);
            _ = db.Coupons.Remove(obj);
            _ = db.SaveChanges();
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }
}
