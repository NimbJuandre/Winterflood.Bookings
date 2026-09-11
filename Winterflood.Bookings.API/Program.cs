using Serilog;
using Winterflood.Bookings.API;
using Winterflood.Bookings.API.Middleware;
using Winterflood.Bookings.Application;
using Winterflood.Bookings.Data;
using Winterflood.Bookings.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, config) => config
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddDataAccess(builder.Configuration)
    .AddApplication()
    .AddApi(builder.Configuration)
    .AddCorsPolicy(builder.Configuration, builder.Environment)
    .AddHealthChecks(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(Winterflood.Bookings.API.DependencyInjection.BookingsCorsPolicy);
app.MapControllers();
app.MapHealthCheckEndpoints();

app.Run();

// Exposed so integration tests can bootstrap the app via WebApplicationFactory if needed.
public partial class Program { }
