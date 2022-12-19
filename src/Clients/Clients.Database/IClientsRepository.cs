using Clients.Model;

namespace Clients.Database;

public interface IClientsRepository
{
    Task<string> CreateNewClientAsync(string firstName,
        string lastName,
        DateOnly birthDate,
        CancellationToken cancellationToken);

    Task<Client> GetClientById(string id, CancellationToken cancellationToken);
}