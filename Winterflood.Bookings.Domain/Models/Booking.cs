namespace Winterflood.Bookings.Domain.Models;

public sealed class Booking
{
    public Guid Id { get; init; }

    public required string CustomerName { get; set; }

    public Enum.BookingType BookingType { get; set; }

    public required string ItemName { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int Quantity { get; set; }
}
