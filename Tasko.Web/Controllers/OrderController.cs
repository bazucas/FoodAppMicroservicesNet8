using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using Tasko.Web.Models;
using Tasko.Web.Service.IService;
using Tasko.Web.Utility;

namespace Tasko.Web.Controllers;

public class OrderController(IOrderService orderService) : Controller
{
    [Authorize]
    public IActionResult OrderIndex()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> OrderDetail(int orderId)
    {
        OrderHeaderDto orderHeaderDto = new();
        string? userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

        var response = await orderService.GetOrder(orderId);
        if (response != null && response.IsSuccess)
        {
            orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
        }
        return !User.IsInRole(SD.RoleAdmin) && userId != orderHeaderDto.UserId ? NotFound() : View(orderHeaderDto);
    }

    [HttpPost("OrderReadyForPickup")]
    public async Task<IActionResult> OrderReadyForPickup(int orderId)
    {
        var response = await orderService.UpdateOrderStatus(orderId, SD.Status_ReadyForPickup);
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Status updated successfully";
            return RedirectToAction(nameof(OrderDetail), new { orderId });
        }
        return View();
    }

    [HttpPost("CompleteOrder")]
    public async Task<IActionResult> CompleteOrder(int orderId)
    {
        var response = await orderService.UpdateOrderStatus(orderId, SD.Status_Completed);
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Status updated successfully";
            return RedirectToAction(nameof(OrderDetail), new { orderId });
        }
        return View();
    }

    [HttpPost("CancelOrder")]
    public async Task<IActionResult> CancelOrder(int orderId)
    {
        var response = await orderService.UpdateOrderStatus(orderId, SD.Status_Cancelled);
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Status updated successfully";
            return RedirectToAction(nameof(OrderDetail), new { orderId });
        }
        return View();
    }


    [HttpGet]
    public IActionResult GetAll(string status)
    {
        IEnumerable<OrderHeaderDto> list;
        string userId = "";
        if (!User.IsInRole(SD.RoleAdmin))
        {
            userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        }
        ResponseDto response = orderService.GetAllOrder(userId).GetAwaiter().GetResult();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(response.Result));
            switch (status)
            {
                case "approved":
                    list = list.Where(u => u.Status == SD.Status_Approved);
                    break;
                case "readyforpickup":
                    list = list.Where(u => u.Status == SD.Status_ReadyForPickup);
                    break;
                case "cancelled":
                    list = list.Where(u => u.Status == SD.Status_Cancelled || u.Status == SD.Status_Refunded);
                    break;
                default:
                    break;
            }
        }
        else
        {
            list = new List<OrderHeaderDto>();
        }
        return Json(new { data = list.OrderByDescending(u => u.OrderHeaderId) });
    }
}
