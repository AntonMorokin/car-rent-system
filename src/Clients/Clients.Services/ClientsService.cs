using Clients.Database;
using Clients.Model;
using Core.Messaging;
using Core.Messaging.Events;

namespace Clients.Services;

internal sealed class ClientsService : IClientsService
{
    private readonly TimeOnly _zero = TimeOnly.MinValue;

    private readonly IClientsRepository _repo;
    private readonly IMessageProducer _messageProducer;

    public ClientsService(IClientsRepository repo, IMessageProducer messageProducer)
    {
        _repo = repo;
        _messageProducer = messageProducer;
    }

    public async Task<string> CreateNewClientAsync(string firstName,
        string lastName,
        DateOnly birthDate,
        CancellationToken cancellationToken)
    {
        if (DateTime.Now < birthDate.AddYears(18).ToDateTime(_zero))
        {
            throw new ArgumentOutOfRangeException(nameof(birthDate),
                birthDate,
                "Client must be older than 18 years to use the system");
        }

        var id = await _repo.CreateNewClientAsync(firstName, lastName, birthDate, cancellationToken);

        await _messageProducer.SendAsync(Consts.Topics.Clients, new ClientEvents.V1.ClientCreated
        {
            ClientId = id,
            FirstName = firstName,
            LastName = lastName,
            BirthDate = birthDate
        });
        
        return id;
    }

    public Task<Client> GetClientByAsync(string id, CancellationToken cancellationToken)
    {
        return _repo.GetClientById(id, cancellationToken);
    }
}