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
            message?.GetType().FullName);

        switch (message)
        {
            case RideEvents.V1.RideCreated created:
                return _carsService.UseInRideAsync(created.CarId, created.RideId, CancellationToken.None);
            default:
                return Task.CompletedTask;
        }
    }
}