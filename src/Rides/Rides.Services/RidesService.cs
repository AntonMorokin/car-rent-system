using Core.Messaging;
using Core.Messaging.Events;
using Rides.Domain.Exceptions;
using Rides.Persistence;
using Aggregate = Rides.Domain.Aggregates.Ride;
using Car = Rides.Domain.Aggregates.Car;
using View = Rides.Domain.Views.Ride;

namespace Rides.Services;

internal sealed class RidesService : IRidesService
{
    private readonly IEventStore<Aggregate> _ridesEventStore;
    private readonly IEventStore<Car> _carsEventStore;
    private readonly IViewStore<View> _ridesViewStore;
    private readonly IMessageProducer _messageProducer;

    public RidesService(IEventStore<Aggregate> ridesEventStore,
        IEventStore<Car> carsEventStore,
        IViewStore<View> ridesViewStore,
        IMessageProducer messageProducer)
    {
        _ridesEventStore = ridesEventStore;
        _carsEventStore = carsEventStore;
        _ridesViewStore = ridesViewStore;
        _messageProducer = messageProducer;
    }

    public Task<string> GetNextIdAsync()
    {
        return _ridesEventStore.GetNextIdAsync();
    }

    public async Task<View> GetRideByIdAsync(string rideId)
    {
        var view = await _ridesViewStore.LoadViewByIdOrDefaultAsync(rideId);
        return view
               ?? throw new DomainException(
                   ErrorCodes.EntityNotFound,
                   $"The ride with Id={rideId} doesn't exists");
    }

    public async Task CreateRideAsync(string rideId, string clientId, string carId, DateTimeOffset createdTime)
    {
        var exists = await _ridesEventStore.CheckIfAggregateExistsAsync(rideId);
        if (exists)
        {
            throw new DomainException(ErrorCodes.EntityAlreadyExists, $"The ride with Id={rideId} already exists");
        }

        var carExists = await _carsEventStore.CheckIfAggregateExistsAsync(carId);
        if (!carExists)
        {
            throw new DomainException(ErrorCodes.EntityNotFound, $"The car with Id={carId} doesn't exist");
        }

        var car = await _carsEventStore.LoadAsync(carId);
        var ride = Aggregate.Create(rideId, clientId, car, createdTime);

        await _ridesEventStore.StoreAsync(ride);

        await _messageProducer.SendAsync(Consts.Topics.Rides, new RideEvents.V1.RideCreated
        {
            RideId = ride.Id,
            ClientId = ride.ClientId,
            CarId = ride.CarId,
            CreatedTime = ride.CreatedTime
        });
    }

    public async Task StartRideAsync(string rideId, DateTimeOffset startedTime)
    {
        var exists = await _ridesEventStore.CheckIfAggregateExistsAsync(rideId);
        if (!exists)
        {
            throw new DomainException(ErrorCodes.EntityNotFound, $"The ride with Id={rideId} doesn't exists");
        }

        var ride = await _ridesEventStore.LoadAsync(rideId);
        var car = await _carsEventStore.LoadAsync(ride.CarId);

        ride.Start(car, startedTime);
        await _ridesEventStore.StoreAsync(ride);
    }
}