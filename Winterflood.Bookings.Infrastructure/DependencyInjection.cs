using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Winterflood.Bookings.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers infrastructure-level concerns (caching, external clients, config bindings)
    /// Currently a placeholder — no external systems are integrated in this solution.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}
