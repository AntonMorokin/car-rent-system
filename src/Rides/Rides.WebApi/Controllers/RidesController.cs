using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rides.Domain.Exceptions;
using Rides.Domain.Views;
using Rides.Services.Commands;
using Rides.Services.Queries;
using Rides.WebApi.Contract.Requests;
using Rides.WebApi.Contract.Responses;

namespace Rides.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class RidesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RidesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("next-id")]
    public Task<string> GetNextIdAsync()
    {
        var query = new GetNextRideIdQuery();
        return _mediator.Send(query);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRideByIdAsync([FromRoute] string id)
    {
        try
        {
            var query = new GetRideByIdQuery
            {
                RideId = id
            };
            var ride = await _mediator.Send(query);
            var response = CreateResponse(ride);

            return Ok(response);
        }
        catch (DomainException ex) when (ex.ErrorCode == ErrorCodes.EntityNotFound)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> CreateRideAsync([FromBody] CreateRideRequest request)
    {
        try
        {
            var command = new CreateNewRideCommand
            {
                RideId = request.RideId,
                ClientId = request.ClientId,
                CarId = request.CarId,
                CreatedTime = request.CreatedTime
            };

            await _mediator.Send(command);

            var url = Url.Action("GetRideById", new { id = request.RideId });
            return Created(url!, request.RideId);
        }
        catch (DomainException ex) when (ex.ErrorCode == ErrorCodes.EntityAlreadyExists)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartRideAsync([FromBody] StartRideRequest request)
    {
        try
        {
            var command = new StartRideCommand
            {
                RideId = request.RideId,
                StartedTime = request.StartedTime
            };

            await _mediator.Send(command);

            var url = Url.Action("GetRideById", new { id = request.RideId });
            return Accepted(url);
        }
        catch (DomainException ex) when (ex.ErrorCode == ErrorCodes.EntityNotFound)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("finish")]
    public async Task<IActionResult> FinishRideAsync([FromBody] FinishRideRequest request)
    {
        try
        {
            var command = new FinishRideCommand
            {
                RideId = request.RideId,
                FinishedTime = request.FinishedTime,
                OdometerReading = request.OdometerReading
            };

            await _mediator.Send(command);

            var url = Url.Action("GetRideById", new { id = request.RideId });
            return Accepted(url);
        }
        catch (DomainException ex) when (ex.ErrorCode == ErrorCodes.EntityNotFound)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("cancel")]
    public async Task<IActionResult> CancelRideAsync([FromBody] CancelRideRequest request)
    {
        try
        {
            var command = new CancelRideCommand
            {
                RideId = request.RideId,
                CancelledTime = request.CancelledTime,
                Reason = request.Reason
            };

            await _mediator.Send(command);

            var url = Url.Action("GetRideById", new { id = request.RideId });
            return Accepted(url);
        }
        catch (DomainException ex) when (ex.ErrorCode == ErrorCodes.EntityNotFound)
        {
            return NotFound(ex.Message);
        }
    }

    private static RideResponse CreateResponse(Ride ride) => new(ride.AggregateId,
        ride.ClientId,
        ride.CarId,
        ride.Status,
        ride.CreatedTime,
        ride.StartedTime,
        ride.FinishedTime,
        ride.OdometerReading,
        ride.CancellationReason);
}