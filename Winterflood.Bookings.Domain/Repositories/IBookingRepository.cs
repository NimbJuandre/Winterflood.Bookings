using Winterflood.Bookings.Domain.Models;

namespace Winterflood.Bookings.Domain.Repositories;

public interface IBookingRepository
{
    Task<Booking?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Booking booking, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
