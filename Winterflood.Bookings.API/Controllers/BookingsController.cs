using Microsoft.AspNetCore.Mvc;
using Winterflood.Bookings.Application.Mapping;
using Winterflood.Bookings.Application.Models.Requests;
using Winterflood.Bookings.Application.Models.Responses;
using Winterflood.Bookings.Application.Services.Interfaces;

namespace Winterflood.Bookings.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class BookingsController(IBookingService bookingService) : ControllerBase
{
    /// <summary>
    /// Creates a new booking.
    /// </summary>
    /// <param name="request">The booking details to create.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The newly created booking.</returns>
    /// <response code="201">Booking was created successfully.</response>
    /// <response code="400">The request payload failed validation.</response>
    [HttpPost]
    [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookingResponse>> Create([FromBody] CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var booking = await bookingService.CreateBookingAsync(request, cancellationToken);
        var response = booking.ToResponse();
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    /// <summary>
    /// Retrieves a booking by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the booking.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The matching booking, if one exists.</returns>
    /// <response code="200">Booking was found and returned.</response>
    /// <response code="404">No booking exists with the specified identifier.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookingResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var booking = await bookingService.GetBookingAsync(id, cancellationToken);
        return booking is null ? NotFound() : Ok(booking.ToResponse());
    }

    /// <summary>
    /// Updates an existing booking.
    /// </summary>
    /// <param name="id">The unique identifier of the booking to update.</param>
    /// <param name="request">The updated booking details.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated booking.</returns>
    /// <response code="200">Booking was updated successfully.</response>
    /// <response code="400">The request payload failed validation.</response>
    /// <response code="404">No booking exists with the specified identifier.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookingResponse>> Update(Guid id, [FromBody] UpdateBookingRequest request, CancellationToken cancellationToken)
    {
        var updated = await bookingService.UpdateBookingAsync(id, request, cancellationToken);
        return updated is null ? NotFound() : Ok(updated.ToResponse());
    }

    /// <summary>
    /// Deletes a booking by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the booking to delete.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <response code="204">Booking was deleted successfully.</response>
    /// <response code="404">No booking exists with the specified identifier.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        return await bookingService.DeleteBookingAsync(id, cancellationToken) ? NoContent() : NotFound();
    }
}
