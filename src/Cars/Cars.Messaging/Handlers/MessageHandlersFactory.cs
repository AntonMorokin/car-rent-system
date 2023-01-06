using Cars.Services;
using Core.Messaging.Handlers;
using Microsoft.Extensions.Logging;

namespace Cars.Messaging.Handlers;

internal static class MessageHandlersFactory
{
    public static IReadOnlyCollection<IMessageHandler> Create(ICarsService carsService, ILoggerFactory loggerFactory)
    {
        var handler = new RideMessagesHandler(carsService, loggerFactory.CreateLogger<RideMessagesHandler>());
        return new[] { handler };
    }
}