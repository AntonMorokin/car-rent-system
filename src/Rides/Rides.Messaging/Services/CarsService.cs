using Rides.Domain.Aggregates;
using Rides.Domain.Exceptions;
using Rides.Persistence;

namespace Rides.Messaging.Services;

internal sealed class CarsService : ICarsService
{
    private readonly IEventStore<Car> _eventStore;

    public CarsService(IEventStore<Car> eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task CreateCarAsync(string carId)
    {
        var exists = await _eventStore.CheckIfAggregateExistsAsync(carId);
        if (exists)
        {
            throw new DomainException(
                ErrorCodes.EntityAlreadyExists,
                $"The car with id={carId} already exists");
        }

        var car = Car.Create(carId);
        await _eventStore.StoreAsync(car);
    }

    public async Task HoldCarAsync(string carId, string rideId)
    {
        var exists = await _eventStore.CheckIfAggregateExistsAsync(carId);
        if (!exists)
        {
            throw new DomainException(
                ErrorCodes.EntityNotFound,
                $"The car with id={carId} doesn't exist");
        }

        var car = await _eventStore.LoadAsync(carId);
        car.StartRide(rideId);
        await _eventStore.StoreAsync(car);
    }

    public async Task FreeCarAsync(string carId)
    {
        var exists = await _eventStore.CheckIfAggregateExistsAsync(carId);
        if (!exists)
        {
            throw new DomainException(
                ErrorCodes.EntityNotFound,
                $"The car with id={carId} doesn't exist");
        }

        var car = await _eventStore.LoadAsync(carId);
        car.FinishRide();
        await _eventStore.StoreAsync(car);
    }
}