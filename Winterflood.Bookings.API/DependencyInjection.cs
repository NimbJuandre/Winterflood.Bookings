using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Winterflood.Bookings.API.Validators;

namespace Winterflood.Bookings.API;

public static class DependencyInjection
{
    public const string BookingsCorsPolicy = "BookingsCorsPolicy";

    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddControllers()
            .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CreateBookingRequestValidator>();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{typeof(DependencyInjection).Assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });
        return services;
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

        services.AddCors(options =>
        {
            options.AddPolicy(BookingsCorsPolicy, policy =>
            {
                if (origins.Length > 0)
                {
                    policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
                }
                else if (environment.IsDevelopment())
                {
                    // Dev convenience only; production must configure Cors:AllowedOrigins.
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks();
        return services;
    }

    public static IEndpointRouteBuilder MapHealthCheckEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/health");
        endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("ready")
        });
        endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false
        });
        return endpoints;
    }
}
