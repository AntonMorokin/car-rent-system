using Core.Messaging.Events;
using Core.Messaging.Handlers;
using Microsoft.Extensions.Logging;
using Rides.Domain.Aggregates;
using Rides.Domain.Exceptions;
using Rides.Persistence;

namespace Rides.Messaging.Handlers;

internal sealed class CarMessagesHandler : IMessageHandler
{
    private readonly IEventStore<Car> _eventStore;
    private readonly ILogger<CarMessagesHandler> _logger;

    public CarMessagesHandler(IEventStore<Car> eventStore, ILogger<CarMessagesHandler> logger)
    {
        _eventStore = eventStore;
        _logger = logger;
    }

    public string Topic => Core.Messaging.Consts.Topics.Cars;

    public async Task HandleAsync(IMessage message)
    {
        _logger.LogDebug(
            "Got for handling message with key={key} of type {type}",
            message.Key,
            message.GetType().FullName);
        
        switch (message)
        {
            case CarEvents.V1.CarCreated created:
                {
                    var exists = await _eventStore.CheckIfAggregateExistsAsync(created.CarId);
                    if (exists)
                    {
                        throw new DomainException(
                            ErrorCodes.EntityAlreadyExists,
                            $"The car with id={created.CarId} already exists");
                    }

                    var car = Car.Create(created.CarId);
                    await _eventStore.StoreAsync(car);
                }
                break;
            case CarEvents.V1.CarHeld held:
                {
                    var exists = await _eventStore.CheckIfAggregateExistsAsync(held.CarId);
                    if (!exists)
                    {
                        throw new DomainException(
                            ErrorCodes.EntityNotFound,
                            $"The car with id={held.CarId} doesn't exist");
                    }

                    var car = await _eventStore.LoadAsync(held.CarId);
                    car.StartRide(held.RideId);
                    await _eventStore.StoreAsync(car);
                }
                break;
            case CarEvents.V1.CarFreed freed:
                {
                    var exists = await _eventStore.CheckIfAggregateExistsAsync(freed.CarId);
                    if (!exists)
                    {
                        throw new DomainException(
                            ErrorCodes.EntityNotFound,
                            $"The car with id={freed.CarId} doesn't exist");
                    }

                    var car = await _eventStore.LoadAsync(freed.CarId);
                    car.FinishRide();
                    await _eventStore.StoreAsync(car);
                }
                break;
            default:
                return;
        }
    }
}