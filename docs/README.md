# API Testing

## Postman

`Winterflood.Bookings.postman_collection.json`

Import the collection into Postman. The `baseUrl` defaults to `http://localhost:5249`.

The Create Booking request saves the returned ID as `bookingId`, so the Get, Update and Delete requests can be run in sequence using the Collection Runner.

## k6 Load Test

`load-tests/get-booking.load.js`

Basic load test for `GET /api/bookings/{id}`. It creates a booking and tests the endpoint with up to 100 virtual users.

### Run

Start the API:

```powershell
dotnet run --project Winterflood.Bookings.API
```

Then run:

```powershell
k6 run docs/load-tests/get-booking.load.js
```

Install k6: https://k6.io/docs/get-started/installation/
