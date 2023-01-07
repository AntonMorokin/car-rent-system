using Cars.Services;
using Core.Messaging;
using Core.Messaging.Events;
using Core.Messaging.Handlers;
using Microsoft.Extensions.Logging;

namespace Cars.Messaging.Handlers;

internal sealed class RideMessagesHandler : IMessageHandler
{
    private readonly ICarsService _carsService;
    private readonly ILogger<RideMessagesHandler> _logger;

    public RideMessagesHandler(ICarsService carsService, ILogger<RideMessagesHandler> logger)
    {
        _carsService = carsService;
        _logger = logger;
    }

    public string HandledTopic => Consts.Topics.Rides;

    public Task HandleAsync(IMessage message)
    {
        _logger.LogDebug(
            "Got for handling message with key={key} of type {type}",
            message.Key,
            message.GetType().FullName);

        return message switch
        {
            RideEvents.V1.RideCreated created => _carsService.UseCarInRideAsync(created.CarId,
                created.RideId,
                CancellationToken.None),
            RideEvents.V1.RideFinished finished => _carsService.FinishRideAsync(finished.CarId,
                finished.OdometerReading,
                CancellationToken.None),
            RideEvents.V1.RideCancelled cancelled => _carsService.CancelRideAsync(cancelled.CarId,
                CancellationToken.None),
            _ => Task.CompletedTask
        };
    }
}