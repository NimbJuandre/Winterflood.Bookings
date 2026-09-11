using Winterflood.Bookings.Domain.Enum;

namespace Winterflood.Bookings.API.Models.Requests;

public sealed class UpdateBookingRequest
{
    public string CustomerName { get; set; } = string.Empty;
    public BookingType BookingType { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int Quantity { get; set; }
}
