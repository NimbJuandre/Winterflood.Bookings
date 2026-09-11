using Winterflood.Bookings.Domain.Enums;

namespace Winterflood.Bookings.Application.Models.Requests;

public sealed class CreateBookingRequest
{
    public string CustomerName { get; set; } = string.Empty;
    public BookingType BookingType { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int Quantity { get; set; }
}
