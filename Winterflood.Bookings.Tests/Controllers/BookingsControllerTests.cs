using Microsoft.AspNetCore.Mvc;
using Moq;
using Winterflood.Bookings.API.Controllers;
using Winterflood.Bookings.API.Models.Requests;
using Winterflood.Bookings.API.Services.Interfaces;
using Winterflood.Bookings.Domain.Enum;
using Winterflood.Bookings.Domain.Models;
using Xunit;

namespace Winterflood.Bookings.Tests.Controllers;

public class BookingsControllerTests
{
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

    private static Booking BookingFrom(CreateBookingRequest r, Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        CustomerName = r.CustomerName,
        BookingType = r.BookingType,
        ItemName = r.ItemName,
        StartDate = r.StartDate,
        EndDate = r.EndDate,
        Quantity = r.Quantity
    };

    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WithBooking()
    {
        var request = ValidCreate();
        var created = BookingFrom(request);
        var service = new Mock<IBookingService>();
        service.Setup(s => s.CreateBookingAsync(request, It.IsAny<CancellationToken>()))
               .ReturnsAsync(created);
        var controller = new BookingsController(service.Object);

        var result = await controller.Create(request, CancellationToken.None);

        var createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(BookingsController.GetById), createdAt.ActionName);
        Assert.Equal(created.Id, Assert.IsType<Booking>(createdAt.Value).Id);
        Assert.Equal(201, createdAt.StatusCode);
    }

    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var booking = BookingFrom(ValidCreate());
        var service = new Mock<IBookingService>();
        service.Setup(s => s.GetBookingAsync(booking.Id, It.IsAny<CancellationToken>()))
               .ReturnsAsync(booking);
        var controller = new BookingsController(service.Object);

        var result = await controller.GetById(booking.Id, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(booking, ok.Value);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var service = new Mock<IBookingService>();
        service.Setup(s => s.GetBookingAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((Booking?)null);
        var controller = new BookingsController(service.Object);

        var result = await controller.GetById(Guid.NewGuid(), CancellationToken.None);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Update_WhenFound_ReturnsOk()
    {
        var id = Guid.NewGuid();
        var request = ValidUpdate();
        var updated = new Booking
        {
            Id = id,
            CustomerName = request.CustomerName,
            BookingType = request.BookingType,
            ItemName = request.ItemName,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Quantity = request.Quantity
        };
        var service = new Mock<IBookingService>();
        service.Setup(s => s.UpdateBookingAsync(id, request, It.IsAny<CancellationToken>()))
               .ReturnsAsync(updated);
        var controller = new BookingsController(service.Object);

        var result = await controller.Update(id, request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(updated, ok.Value);
    }

    [Fact]
    public async Task Update_WhenMissing_ReturnsNotFound()
    {
        var service = new Mock<IBookingService>();
        service.Setup(s => s.UpdateBookingAsync(It.IsAny<Guid>(), It.IsAny<UpdateBookingRequest>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((Booking?)null);
        var controller = new BookingsController(service.Object);

        var result = await controller.Update(Guid.NewGuid(), ValidUpdate(), CancellationToken.None);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Delete_WhenFound_ReturnsNoContent()
    {
        var id = Guid.NewGuid();
        var service = new Mock<IBookingService>();
        service.Setup(s => s.DeleteBookingAsync(id, It.IsAny<CancellationToken>()))
               .ReturnsAsync(true);
        var controller = new BookingsController(service.Object);

        var result = await controller.Delete(id, CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_WhenMissing_ReturnsNotFound()
    {
        var service = new Mock<IBookingService>();
        service.Setup(s => s.DeleteBookingAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(false);
        var controller = new BookingsController(service.Object);

        var result = await controller.Delete(Guid.NewGuid(), CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }
}
