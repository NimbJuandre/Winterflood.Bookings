using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Winterflood.Bookings.Data.Repository.Implementation;
using Winterflood.Bookings.Data.Repository.Interfaces;

namespace Winterflood.Bookings.Data;

public static class DependencyInjection
{
    /// <summary>
    /// Registers data-access components. In-memory repository is a singleton so all requests share state.
    /// </summary>
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
        return services;
    }
}
