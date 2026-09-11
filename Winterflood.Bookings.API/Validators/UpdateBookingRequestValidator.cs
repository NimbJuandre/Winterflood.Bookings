using FluentValidation;
using Winterflood.Bookings.Application.Models.Requests;

namespace Winterflood.Bookings.API.Validators;

public sealed class UpdateBookingRequestValidator : AbstractValidator<UpdateBookingRequest>
{
    public UpdateBookingRequestValidator()
    {
        RuleFor(x => x.CustomerName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.BookingType).IsInEnum();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("EndDate cannot be before StartDate.");
    }
}
