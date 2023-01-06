using Core.Messaging.Handlers;
using Microsoft.Extensions.Logging;
using Rides.Persistence;

namespace Rides.Messaging.Handlers;

internal static class MessageHandlersFactory
{
    public static IReadOnlyCollection<IMessageHandler> CreateCarMessagesHandlers(IEventStore<Domain.Aggregates.Car> carsEventStore,
        ILoggerFactory loggerFactory)
    {
        var handler = new CarMessagesHandler(carsEventStore, loggerFactory.CreateLogger<CarMessagesHandler>());
        return new[] { handler };
    }
}