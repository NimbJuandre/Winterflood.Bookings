# Winterflood Bookings API

A small ASP.NET Core Web API built with .NET 10 for creating, retrieving, updating and deleting bookings.

The solution is intentionally kept simple and focused on the assessment requirements. Bookings are stored in memory and no database is required.

## Running

```bash
dotnet run --project Winterflood.Bookings.API
```

Swagger is available in Development at:

```text
https://localhost:7298/swagger
http://localhost:5249/swagger
```

Run the tests:

```bash
dotnet test
```

### Health checks

| Route           | Purpose        |
| --------------- | -------------- |
| `/health`       | Overall health |
| `/health/ready` | Readiness      |
| `/health/live`  | Liveness       |

## API Endpoints

| Method | Route                | Description      | Success | Errors   |
| ------ | -------------------- | ---------------- | ------- | -------- |
| POST   | `/api/bookings`      | Create a booking | 201     | 400      |
| GET    | `/api/bookings/{id}` | Get a booking    | 200     | 404      |
| PUT    | `/api/bookings/{id}` | Update a booking | 200     | 400, 404 |
| DELETE | `/api/bookings/{id}` | Delete a booking | 204     | 404      |

## Solution Structure

The solution is split into separate layers:

```text
Winterflood.Bookings.API            HTTP layer, controllers, middleware and configuration
Winterflood.Bookings.Application    Services, DTOs and application logic
Winterflood.Bookings.Domain         Domain entities and enums
Winterflood.Bookings.Data           Repository abstraction and in-memory storage
Winterflood.Bookings.Infrastructure External infrastructure concerns
Winterflood.Bookings.Tests          Unit tests
```

## Features

- **FluentValidation** – Request and cross-field validation.
- **Serilog** – Structured console and request logging.
- **Swagger / OpenAPI** – API documentation and testing.
- **Health checks** – Basic health, readiness and liveness endpoints.
- **CORS** – Environment-aware configuration through `Cors:AllowedOrigins`.
- **String enums** – `BookingType` is represented as a readable string.

## Testing

The `docs/` folder contains:

- `docs/Winterflood.Bookings.postman_collection.json` – CRUD and health check requests.
- `docs/load-tests/get-booking.load.js` – k6 load test for the GET endpoint.

See `docs/README.md` for instructions.

### What would I do differently in a production system?

- SQL database with migrations, proper indexing and backups.
- EF Core / Dapper depending on the data-access requirements. EF Core would be the default, with Dapper for cases where direct SQL is justified.
- Availability and concurrency control to prevent double-booking and handle simultaneous requests safely.
- Redis for caching frequently accessed data where it provides a measurable benefit.
- Authentication and authorisation using JWT/OAuth2 with role or policy-based access control where required.
- Rate limiting and request limits to protect the API from abuse.
- Idempotency for operations such as booking creation to safely handle client retries.
- Observability with structured logging, metrics, distributed tracing and alerting.
- Resilience around external dependencies, including timeouts, retries and circuit breakers where appropriate.
- CI/CD with automated builds, tests, security checks and deployments.
- Containerisation with Docker and appropriate runtime configuration.
- Secrets management using a secure secret store rather than configuration files.
- Integration and end-to-end tests covering the API, database and important booking scenarios.
- Messaging for booking events and background processing where asynchronous work is appropriate.
- API versioning and backwards compatibility as the API evolves.


