using Clients.Model;

namespace Clients.Services;

public interface IClientsService
{
    Task<string> CreateNewClientAsync(string firstName,
        string lastName,
        DateOnly birthDate,
        CancellationToken cancellationToken);

    Task<Client> GetClientByAsync(string id, CancellationToken cancellationToken);
}