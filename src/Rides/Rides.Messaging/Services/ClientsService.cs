using Rides.Domain.Aggregates;
using Rides.Domain.Exceptions;
using Rides.Persistence;

namespace Rides.Messaging.Services;

internal sealed class ClientsService : IClientsService
{
    private readonly IEventStore<Client> _eventStore;

    public ClientsService(IEventStore<Client> eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task CreateClientAsync(string clientId, string firstName, string lastName, DateOnly birthDate)
    {
        var exists = await _eventStore.CheckIfAggregateExistsAsync(clientId);
        if (exists)
        {
            throw new DomainException(
                ErrorCodes.EntityAlreadyExists,
                $"The client with id={clientId} already exists");
        }

        var client = Client.Create(clientId, firstName, lastName, birthDate);
        await _eventStore.StoreAsync(client);
    }
}