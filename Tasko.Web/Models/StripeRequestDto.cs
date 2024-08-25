namespace Tasko.Web.Models;

public class StripeRequestDto
{
    public string? StripeSessionUrl { get; set; }
    public string? StripeSessionId { get; set; }
    public required string ApprovedUrl { get; set; }
    public required string CancelUrl { get; set; }
    public required OrderHeaderDto OrderHeader { get; set; }
}
