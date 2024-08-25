using Tasko.Web.Models;
using Tasko.Web.Service.IService;
using Tasko.Web.Utility;

namespace Tasko.Web.Service;

public class OrderService(IBaseService baseService) : IOrderService
{
    public async Task<ResponseDto?> CreateOrder(CartDto cartDto)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = cartDto,
            Url = SD.OrderAPIBase + "/api/order/CreateOrder"
        });
    }

    public async Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = stripeRequestDto,
            Url = SD.OrderAPIBase + "/api/order/CreateStripeSession"
        });
    }

    public async Task<ResponseDto?> GetAllOrder(string? userId)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.OrderAPIBase + "/api/order/GetOrders?userId=" + userId
        });
    }

    public async Task<ResponseDto?> GetOrder(int orderId)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.OrderAPIBase + "/api/order/GetOrder/" + orderId
        });
    }

    public async Task<ResponseDto?> UpdateOrderStatus(int orderId, string newStatus)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = newStatus,
            Url = SD.OrderAPIBase + "/api/order/UpdateOrderStatus/" + orderId
        });
    }

    public async Task<ResponseDto?> ValidateStripeSession(int orderHeaderId)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = orderHeaderId,
            Url = SD.OrderAPIBase + "/api/order/ValidateStripeSession"
        });
    }
}
