using Winterflood.Bookings.Domain.Enums;

namespace Winterflood.Bookings.Application.Models.Responses;

/// <summary>
/// API-facing representation of a booking. Kept separate from the
/// <see cref="Domain.Models.Booking"/> domain entity so the public contract can
/// evolve independently of the internal model.
/// </summary>
public sealed record BookingResponse(
    Guid Id,
    string CustomerName,
    BookingType BookingType,
    string ItemName,
    DateOnly StartDate,
    DateOnly EndDate,
    int Quantity);
