using Winterflood.Bookings.Application.Models.Responses;
using Winterflood.Bookings.Domain.Models;

namespace Winterflood.Bookings.Application.Mapping;

public static class BookingMappingExtensions
{
    public static BookingResponse ToResponse(this Booking booking) => new(
        booking.Id,
        booking.CustomerName,
        booking.BookingType,
        booking.ItemName,
        booking.StartDate,
        booking.EndDate,
        booking.Quantity);
}
