using Clients.Database;
using Clients.Model;

namespace Clients.Services;

internal sealed class ClientsService : IClientsService
{
    private readonly TimeOnly _zero = TimeOnly.MinValue;

    private readonly IClientsRepository _repo;

    public ClientsService(IClientsRepository repo)
    {
        _repo = repo;
    }

    public Task<string> CreateNewClientAsync(string firstName,
        string lastName,
        DateOnly birthDate,
        CancellationToken cancellationToken)
    {
        if (DateTime.Now < birthDate.AddYears(18).ToDateTime(_zero))
        {
            throw new ArgumentException("Client must be older than 18 years to use the system", nameof(birthDate));
        }

        return _repo.CreateNewClientAsync(firstName, lastName, birthDate, cancellationToken);
    }

    public Task<Client> GetClientByAsync(string id, CancellationToken cancellationToken)
    {
        return _repo.GetClientById(id, cancellationToken);
    }
}