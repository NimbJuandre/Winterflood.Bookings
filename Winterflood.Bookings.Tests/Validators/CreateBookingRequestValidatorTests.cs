using FluentValidation.TestHelper;
using Winterflood.Bookings.API.Models.Requests;
using Winterflood.Bookings.API.Validators;
using Winterflood.Bookings.Domain.Enum;
using Xunit;

namespace Winterflood.Bookings.Tests.Validators;

public class CreateBookingRequestValidatorTests
{
    private readonly CreateBookingRequestValidator _validator = new();

    private static CreateBookingRequest Valid() => new()
    {
        CustomerName = "Alice",
        BookingType = BookingType.Apartment,
        ItemName = "Seaside flat",
        StartDate = new DateOnly(2026, 6, 1),
        EndDate = new DateOnly(2026, 6, 7),
        Quantity = 1
    };

    [Fact]
    public void ValidRequest_PassesValidation()
    {
        _validator.TestValidate(Valid()).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void EmptyCustomerName_Fails()
    {
        var req = Valid();
        req.CustomerName = string.Empty;

        _validator.TestValidate(req).ShouldHaveValidationErrorFor(x => x.CustomerName);
    }

    [Fact]
    public void EmptyItemName_Fails()
    {
        var req = Valid();
        req.ItemName = string.Empty;

        _validator.TestValidate(req).ShouldHaveValidationErrorFor(x => x.ItemName);
    }

    [Fact]
    public void QuantityZeroOrLess_Fails()
    {
        var req = Valid();
        req.Quantity = 0;

        _validator.TestValidate(req).ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Fact]
    public void InvalidBookingType_Fails()
    {
        var req = Valid();
        req.BookingType = (BookingType)999;

        _validator.TestValidate(req).ShouldHaveValidationErrorFor(x => x.BookingType);
    }

    [Fact]
    public void EndDateBeforeStartDate_Fails()
    {
        var req = Valid();
        req.StartDate = new DateOnly(2026, 6, 10);
        req.EndDate = new DateOnly(2026, 6, 1);

        _validator.TestValidate(req).ShouldHaveValidationErrorFor(x => x.EndDate);
    }
}
