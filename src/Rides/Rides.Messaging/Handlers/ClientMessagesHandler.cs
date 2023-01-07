using Core.Messaging;
using Core.Messaging.Events;
using Core.Messaging.Handlers;
using Microsoft.Extensions.Logging;
using Rides.Messaging.Services;

namespace Rides.Messaging.Handlers;

internal sealed class ClientMessagesHandler : IMessageHandler
{
    private readonly IClientsService _clientsService;
    private readonly ILogger<ClientMessagesHandler> _logger;

    public ClientMessagesHandler(IClientsService clientsService, ILogger<ClientMessagesHandler> logger)
    {
        _clientsService = clientsService;
        _logger = logger;
    }

    public string HandledTopic => Consts.Topics.Clients;
    
    public Task HandleAsync(IMessage message)
    {
        _logger.LogDebug(
            "Got for handling message with key={key} of type {type}",
            message.Key,
            message.GetType().FullName);
        
        return message switch
        {
            ClientEvents.V1.ClientCreated created => _clientsService.CreateClientAsync(created.ClientId,
                created.FirstName,
                created.LastName,
                created.BirthDate),
            _ => Task.CompletedTask
        };
    }
}