using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rides.Domain;
using Rides.Persistence;
using Rides.WebApi.Contract.Requests;
using Rides.WebApi.Contract.Responses;

namespace Rides.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class RidesController : ControllerBase
{
    private readonly IEventStore<Ride> _eventStore;

    public RidesController(IEventStore<Ride> eventStore)
    {
        _eventStore = eventStore;
    }

    [HttpGet("next-id")]
    public Task<string> GetNextIdAsync()
    {
        return _eventStore.GetNextIdAsync();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRideByIdAsync([FromRoute] string id)
    {
        var exists = await _eventStore.CheckIfAggregateExistsAsync(id);
        if (!exists)
        {
            return NotFound($"Unable to find the ride with id={id}");
        }

        var ride = await _eventStore.LoadAsync(id);
        var response = CreateResponse(ride);

        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> CreateRideAsync([FromBody] CreateRideRequest request)
    {
        var exists = await _eventStore.CheckIfAggregateExistsAsync(request.RideId);
        if (exists)
        {
            return Conflict($"The ride with id={request.RideId} already exists");
        }

        var ride = Ride.Create(request.RideId, request.ClientId, request.CarId, request.CreatedTime);
        await _eventStore.StoreAsync(ride);

        var url = Url.Action("GetRideById", new { id = ride.Id });
        return Created(url!, ride.Id);
    }

    [HttpPut("start")]
    public async Task<IActionResult> StartRideAsync([FromBody] StartRideRequest request)
    {
        var exists = await _eventStore.CheckIfAggregateExistsAsync(request.RideId);
        if (!exists)
        {
            return NotFound($"The ride with id={request.RideId} doesn't exists");
        }

        var ride = await _eventStore.LoadAsync(request.RideId);
        ride.Start(request.StartedTime);
        await _eventStore.StoreAsync(ride);
        
        var url = Url.Action("GetRideById", new { id = ride.Id });
        return Accepted(url);
    }

    private static RideResponse CreateResponse(Ride ride) => new(ride.Id,
        ride.ClientId,
        ride.CarId,
        ride.Status.ToString(),
        ride.CreatedTime,
        ride.StartedTime,
        ride.FinishedTime,
        ride.OdometerReading,
        ride.CancellationReason);
}