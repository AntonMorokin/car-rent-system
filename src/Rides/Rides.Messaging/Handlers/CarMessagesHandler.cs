using Core.Messaging.Events;
using Core.Messaging.Handlers;
using Microsoft.Extensions.Logging;
using Rides.Messaging.Services;

namespace Rides.Messaging.Handlers;

internal sealed class CarMessagesHandler : IMessageHandler
{
    private readonly ICarsService _carsService;
    private readonly ILogger<CarMessagesHandler> _logger;

    public CarMessagesHandler(ICarsService carsService, ILogger<CarMessagesHandler> logger)
    {
        _logger = logger;
        _carsService = carsService;
    }

    public string HandledTopic => Core.Messaging.Consts.Topics.Cars;

    public Task HandleAsync(IMessage message)
    {
        _logger.LogDebug(
            "Got for handling message with key={key} of type {type}",
            message.Key,
            message.GetType().FullName);

        return message switch
        {
            CarEvents.V1.CarCreated created => _carsService.CreateCarAsync(created.CarId),
            CarEvents.V1.CarHeld held => _carsService.HoldCarAsync(held.CarId, held.RideId),
            CarEvents.V1.CarFreed freed => _carsService.FreeCarAsync(freed.CarId),
            _ => Task.CompletedTask
        };
    }
}