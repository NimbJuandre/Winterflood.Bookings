using Microsoft.Extensions.Logging.Abstractions;
using Winterflood.Bookings.Application.Models.Requests;
using Winterflood.Bookings.Application.Services.Implementation;
using Winterflood.Bookings.Data.Repositories;
using Winterflood.Bookings.Domain.Enums;
using Xunit;

namespace Winterflood.Bookings.Tests.Services;

public class BookingServiceTests
{
    private static BookingService CreateService() =>
        new(new BookingRepository(), NullLogger<BookingService>.Instance);

    private static CreateBookingRequest ValidCreate() => new()
    {
        CustomerName = "Alice",
        BookingType = BookingType.Apartment,
        ItemName = "Seaside flat",
        StartDate = new DateOnly(2026, 6, 1),
        EndDate = new DateOnly(2026, 6, 7),
        Quantity = 1
    };

    private static UpdateBookingRequest ValidUpdate() => new()
    {
        CustomerName = "Alice B.",
        BookingType = BookingType.Apartment,
        ItemName = "Mountain cabin",
        StartDate = new DateOnly(2026, 7, 1),
        EndDate = new DateOnly(2026, 7, 5),
        Quantity = 2
    };

    [Fact]
    public async Task CreateBooking_ReturnsCreatedBooking()
    {
        var service = CreateService();

        var booking = await service.CreateBookingAsync(ValidCreate());

        Assert.NotEqual(Guid.Empty, booking.Id);
        Assert.Equal("Alice", booking.CustomerName);
        Assert.Equal(BookingType.Apartment, booking.BookingType);
        Assert.Equal("Seaside flat", booking.ItemName);
        Assert.Equal(1, booking.Quantity);
    }

    [Fact]
    public async Task GetBooking_ReturnsBooking()
    {
        var service = CreateService();
        var created = await service.CreateBookingAsync(ValidCreate());

        var fetched = await service.GetBookingAsync(created.Id);

        Assert.NotNull(fetched);
        Assert.Equal(created.Id, fetched!.Id);
    }

    [Fact]
    public async Task GetBooking_WhenBookingDoesNotExist_ReturnsNull()
    {
        var service = CreateService();

        var result = await service.GetBookingAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateBooking_UpdatesExistingBooking()
    {
        var service = CreateService();
        var created = await service.CreateBookingAsync(ValidCreate());

        var updated = await service.UpdateBookingAsync(created.Id, ValidUpdate());

        Assert.NotNull(updated);
        Assert.Equal(created.Id, updated!.Id);
        Assert.Equal("Alice B.", updated.CustomerName);
        Assert.Equal("Mountain cabin", updated.ItemName);
        Assert.Equal(2, updated.Quantity);
        Assert.Equal(new DateOnly(2026, 7, 1), updated.StartDate);
    }

    [Fact]
    public async Task UpdateBooking_WhenBookingDoesNotExist_ReturnsNull()
    {
        var service = CreateService();

        var result = await service.UpdateBookingAsync(Guid.NewGuid(), ValidUpdate());

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteBooking_DeletesExistingBooking()
    {
        var service = CreateService();
        var created = await service.CreateBookingAsync(ValidCreate());

        var deleted = await service.DeleteBookingAsync(created.Id);

        Assert.True(deleted);
        Assert.Null(await service.GetBookingAsync(created.Id));
    }

    [Fact]
    public async Task DeleteBooking_WhenBookingDoesNotExist_ReturnsFalse()
    {
        var service = CreateService();

        Assert.False(await service.DeleteBookingAsync(Guid.NewGuid()));
    }
}
