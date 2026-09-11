# Winterflood Bookings API

A small ASP.NET Core Web API (.NET 10) for creating, retrieving, updating and deleting
bookings for holiday-related items (apartments, vehicles, shows, ...).

Built as a technical assessment: the goal is a clean, well-structured, easy-to-explain
solution rather than a fully productionised system.

## What the application does

Exposes a REST API that allows a caller to:

- Create a booking
- Retrieve a booking
- Update a booking
- Delete a booking

Bookings are stored in memory. No database is used.

## Running

```bash
cd Winterflood.Bookings
dotnet run
```

Swagger UI is available (Development environment) at:

```
https://localhost:{port}/swagger
```

Run the tests:

```bash
dotnet test
```

## API endpoints

| Method | Route                  | Description        | Success | Errors            |
|--------|------------------------|--------------------|---------|-------------------|
| POST   | `/api/bookings`        | Create a booking   | 201     | 400               |
| GET    | `/api/bookings/{id}`   | Get a booking      | 200     | 404               |
| PUT    | `/api/bookings/{id}`   | Update a booking   | 200     | 400, 404          |
| DELETE | `/api/bookings/{id}`   | Delete a booking   | 204     | 404               |

### Example: create a booking

```http
POST /api/bookings
Content-Type: application/json

{
  "customerName": "Alice",
  "bookingType": "Apartment",
  "itemName": "Seaside flat",
  "startDate": "2026-06-01",
  "endDate": "2026-06-07",
  "quantity": 1
}
```

Response `201 Created` with `Location: /api/bookings/{id}` and the created booking body.

### Example: update a booking

```http
PUT /api/bookings/{id}
Content-Type: application/json

{
  "customerName": "Alice B.",
  "bookingType": "Apartment",
  "itemName": "Mountain cabin",
  "startDate": "2026-07-01",
  "endDate": "2026-07-05",
  "quantity": 2
}
```

## Architecture

```
Controllers  →  Services  →  Repository interface  →  In-memory repository
```

- **Controllers** – HTTP concerns only (routing, status codes, request binding).
- **Services** – Business/application logic and cross-field validation.
- **Repository** – Storage abstraction (`IBookingRepository`) with a
  `ConcurrentDictionary`-backed in-memory implementation.
- **DTOs** – Separate request models keep the API contract decoupled from the domain model.
- **Middleware** – A small exception-handling middleware maps expected exceptions
  (e.g. `ValidationException`) to RFC 7807 `ProblemDetails` responses and logs the rest.

## Architecture Decisions

### No database
The assessment explicitly states that persistence is not required, so a database would
introduce unnecessary complexity.

### Repository abstraction
The repository is hidden behind `IBookingRepository`, so a persistent store (SQL, Mongo,
etc.) could be swapped in later without touching the service or controller.

### ConcurrentDictionary
The API can receive concurrent HTTP requests. `ConcurrentDictionary<Guid, Booking>` gives
thread-safe reads/writes without any explicit locking in the repository.

### Service layer
Business logic lives in `BookingService`, not the controller, so it is trivial to unit test
and the controller stays focused on HTTP.

### DTOs separate from the domain model
`CreateBookingRequest` / `UpdateBookingRequest` are separate from `Booking`, so the
public API contract is not tightly coupled to the internal representation. It also lets
the domain model own its identity (`Id` is `init`-only) while requests remain free of it.

### Validation via DataAnnotations
For a small API, DataAnnotations plus a small amount of business validation in the service
(`EndDate >= StartDate`) is enough. `[ApiController]` automatically returns
`400 Bad Request` with a `ValidationProblemDetails` body when the model is invalid.
FluentValidation would be justifiable on a larger project.

### Global exception handling
A single middleware translates known exceptions to `ProblemDetails` and logs unexpected
ones, so controllers do not need repetitive `try/catch` blocks.

### `Guid` ids
Ids are generated server-side so clients cannot collide or forge them, and the client
never needs to supply an id on create.

## Production considerations (not implemented)

If this were a real booking system I would additionally consider:

- **Persistent database** (e.g. PostgreSQL) with EF Core or Dapper.
- **Availability / capacity checking** – bookings would compete for real inventory.
- **Concurrency control** – optimistic concurrency via a row version / ETag.
- **AuthN / AuthZ** – JWT bearer auth, per-customer authorisation.
- **Idempotency keys** on `POST` to protect against retries creating duplicates.
- **Structured logging + metrics + distributed tracing** (Serilog, OpenTelemetry).
- **Integration tests** using `WebApplicationFactory<Program>`.
- **Distributed caching** for hot reads (e.g. Redis) once persisted.
- **Booking confirmation events** published to a message broker for downstream systems.
- **Message queues** for async work such as sending emails or reconciling inventory.
- **Rate limiting** and input size limits at the edge.
- **CI/CD** with automated build, test and container publish.

None of these are needed for the assessment; they are listed to show awareness of what a
real system would require.
