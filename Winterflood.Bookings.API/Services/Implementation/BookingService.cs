using Winterflood.Bookings.API.Models.Requests;
using Winterflood.Bookings.API.Services.Interfaces;
using Winterflood.Bookings.Data.Repository.Interfaces;
using Winterflood.Bookings.Domain.Models;

namespace Winterflood.Bookings.API.Services.Implementation;

public sealed class BookingService(IBookingRepository repository, ILogger<BookingService> logger) : IBookingService
{
    public async Task<Booking> CreateBookingAsync(CreateBookingRequest request, CancellationToken cancellationToken = default)
    {
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            CustomerName = request.CustomerName.Trim(),
            BookingType = request.BookingType,
            ItemName = request.ItemName.Trim(),
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Quantity = request.Quantity
        };

        await repository.AddAsync(booking, cancellationToken);
        logger.LogInformation("Created booking {BookingId} for {CustomerName}", booking.Id, booking.CustomerName);
        return booking;
    }

    public Task<Booking?> GetBookingAsync(Guid id, CancellationToken cancellationToken = default) =>
        repository.GetAsync(id, cancellationToken);

    public async Task<Booking?> UpdateBookingAsync(Guid id, UpdateBookingRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await repository.GetAsync(id, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        existing.CustomerName = request.CustomerName.Trim();
        existing.BookingType = request.BookingType;
        existing.ItemName = request.ItemName.Trim();
        existing.StartDate = request.StartDate;
        existing.EndDate = request.EndDate;
        existing.Quantity = request.Quantity;

        await repository.UpdateAsync(existing, cancellationToken);
        logger.LogInformation("Updated booking {BookingId}", id);
        return existing;
    }

    public async Task<bool> DeleteBookingAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var deleted = await repository.DeleteAsync(id, cancellationToken);
        if (deleted)
        {
            logger.LogInformation("Deleted booking {BookingId}", id);
        }
        return deleted;
    }
}
