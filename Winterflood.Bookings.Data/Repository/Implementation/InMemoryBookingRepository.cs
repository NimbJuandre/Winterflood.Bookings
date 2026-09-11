using System.Collections.Concurrent;
using Winterflood.Bookings.Data.Repository.Interfaces;
using Winterflood.Bookings.Domain.Models;

namespace Winterflood.Bookings.Data.Repository.Implementation;

/// <summary>
/// Thread-safe in-memory booking store.
/// Uses <see cref="ConcurrentDictionary{TKey,TValue}"/> because the API can be hit by
/// concurrent HTTP requests and would otherwise race on writes.
/// </summary>
public sealed class InMemoryBookingRepository : IBookingRepository
{
    private readonly ConcurrentDictionary<Guid, Booking> _bookings = new();

    public Task<Booking?> GetAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_bookings.TryGetValue(id, out var booking) ? booking : null);

    public Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        if (!_bookings.TryAdd(booking.Id, booking))
        {
            // Practically unreachable because Id is a freshly generated Guid, but we
            // fail loudly rather than silently overwrite.
            throw new InvalidOperationException($"Booking {booking.Id} already exists.");
        }
        return Task.CompletedTask;
    }

    public Task<bool> UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        if (!_bookings.ContainsKey(booking.Id))
        {
            return Task.FromResult(false);
        }

        _bookings[booking.Id] = booking;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_bookings.TryRemove(id, out _));
}
