using Winterflood.Bookings.Application.Models.Requests;
using Winterflood.Bookings.Domain.Models;

namespace Winterflood.Bookings.Application.Services.Interfaces;

public interface IBookingService
{
    Task<Booking> CreateBookingAsync(CreateBookingRequest request, CancellationToken cancellationToken = default);
    Task<Booking?> GetBookingAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Booking?> UpdateBookingAsync(Guid id, UpdateBookingRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteBookingAsync(Guid id, CancellationToken cancellationToken = default);
}
