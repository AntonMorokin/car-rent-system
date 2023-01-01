using Rides.Domain.Aggregates;
using Rides.Domain.Exceptions;
using Rides.Persistence;

namespace Rides.Services;

internal sealed class RidesWriteService : IRidesWriteService
{
    private readonly IEventStore<Ride> _eventStore;

    public RidesWriteService(IEventStore<Ride> eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task CreateRideAsync(string rideId, string clientId, string carId, DateTimeOffset createdTime)
    {
        var exists = await _eventStore.CheckIfAggregateExistsAsync(rideId);
        if (exists)
        {
            throw new DomainException(ErrorCodes.EntityAlreadyExists, $"The ride with Id={rideId} already exists");
        }

        var ride = Ride.Create(rideId, clientId, carId, createdTime);
        await _eventStore.StoreAsync(ride);
    }

    public async Task StartRideAsync(string rideId, DateTimeOffset startedTime)
    {
        var exists = await _eventStore.CheckIfAggregateExistsAsync(rideId);
        if (!exists)
        {
            throw new DomainException(ErrorCodes.EntityNotFound, $"The ride with Id={rideId} doesn't exists");
        }

        var ride = await _eventStore.LoadAsync(rideId);
        ride.Start(startedTime);
        await _eventStore.StoreAsync(ride);
    }
}