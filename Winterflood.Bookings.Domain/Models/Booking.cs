namespace Winterflood.Bookings.Domain.Models;

using Winterflood.Bookings.Domain.Enums;

public sealed class Booking
{
    public Guid Id { get; init; }

    public required string CustomerName { get; set; }

    public BookingType BookingType { get; set; }

    public required string ItemName { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int Quantity { get; set; }
}
