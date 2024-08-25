using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Tasko.Web.Models;
using Tasko.Web.Service.IService;

namespace Tasko.Web.Controllers;

public class CouponController(ICouponService couponService) : Controller
{
    public async Task<IActionResult> CouponIndex()
    {
        List<CouponDto>? list = [];

        ResponseDto? response = await couponService.GetAllCouponsAsync();

        if (response is { IsSuccess: true })
        {
            list = JsonSerializer.Deserialize<List<CouponDto>>(Convert.ToString(response.Result) ?? string.Empty);
        }
        else
        {
            TempData["error"] = response?.Message;
        }

        return View(list);
    }

    public IActionResult CouponCreate()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CouponCreate(CouponDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        ResponseDto? response = await couponService.CreateCouponsAsync(model);

        if (response is { IsSuccess: true })
        {
            TempData["success"] = "Coupon created successfully";
            return RedirectToAction(nameof(CouponIndex));
        }

        TempData["error"] = response?.Message;
        return View(model);
    }

    public async Task<IActionResult> CouponDelete(int couponId)
    {
        ResponseDto? response = await couponService.GetCouponByIdAsync(couponId);

        if (response is { IsSuccess: true })
        {
            CouponDto? model = JsonSerializer.Deserialize<CouponDto>(Convert.ToString(response.Result) ?? string.Empty);
            return View(model);
        }

        TempData["error"] = response?.Message;
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CouponDelete(CouponDto couponDto)
    {
        ResponseDto? response = await couponService.DeleteCouponsAsync(couponDto.CouponId);

        if (response is { IsSuccess: true })
        {
            TempData["success"] = "Coupon deleted successfully";
            return RedirectToAction(nameof(CouponIndex));
        }

        TempData["error"] = response?.Message;
        return View(couponDto);
    }
}
