using Core.Messaging.Handlers;
using Microsoft.Extensions.Logging;
using Rides.Messaging.Services;

namespace Rides.Messaging.Handlers;

internal static class MessageHandlersFactory
{
    public static IReadOnlyCollection<IMessageHandler> CreateCarMessagesHandlers(ICarsService carsService,
        IClientsService clientsService,
        ILoggerFactory loggerFactory)
    {
        var carsHandler = new CarMessagesHandler(carsService, loggerFactory.CreateLogger<CarMessagesHandler>());
        var clientsHandler = new ClientMessagesHandler(clientsService, loggerFactory.CreateLogger<ClientMessagesHandler>());
        
        return new IMessageHandler[] { carsHandler, clientsHandler };
    }
}