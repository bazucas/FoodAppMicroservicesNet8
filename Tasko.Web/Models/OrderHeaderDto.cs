﻿namespace Tasko.Web.Models;

public class OrderHeaderDto
{
    public int OrderHeaderId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string CouponCode { get; set; } = string.Empty;
    public double Discount { get; set; }
    public double OrderTotal { get; set; }


    public string Name { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public DateTime OrderTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentIntentId { get; set; } = string.Empty;
    public string StripeSessionId { get; set; } = string.Empty;
    public IEnumerable<OrderDetailsDto> OrderDetails { get; set; } = new List<OrderDetailsDto>();
}
