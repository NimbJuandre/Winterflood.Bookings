using Microsoft.Extensions.DependencyInjection;
using Winterflood.Bookings.Application.Services.Implementation;
using Winterflood.Bookings.Application.Services.Interfaces;

namespace Winterflood.Bookings.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBookingService, BookingService>();
        return services;
    }
}
