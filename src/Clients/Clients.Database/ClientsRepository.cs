using Clients.Model;
using Dapper;
using Npgsql;

namespace Clients.Database;

public sealed class ClientsRepository : IClientsRepository
{
    private readonly string _connectionString;

    public ClientsRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<string> CreateNewClientAsync(string firstName,
        string lastName,
        DateOnly birthDate,
        CancellationToken cancellationToken)
    {
        const string Command = "insert into clients_ms.clients(first_name, last_name, birth_date)"
                               + " values(@firstName, @lastName, @birthDate) returning id";

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var @params = new
        {
            firstName,
            lastName,
            birthDate
        };

        return await connection.QuerySingleAsync<string>(Command, @params);
    }

    public async Task<Client> GetClientById(string id, CancellationToken cancellationToken)
    {
        const string Command = "select id as \"Id\", first_name as \"FirstName\", last_name as \"LastName\", birth_date as \"BirthDate\""
                               + " from clients_ms.clients where id = @id";

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var @params = new
        {
            id = Convert.ToInt32(id)
        };

        var data = await connection.QueryAsync<Model.Client>(Command, @params);
        using var enumerator = data.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            throw new ArgumentException($"There is no Client with id={id}", nameof(id));
        }

        var client = enumerator.Current;

        if (enumerator.MoveNext())
        {
            throw new ArgumentException($"There is no several Clients with id={id}", nameof(id));
        }

        return new Client
        {
            Id = client.Id.ToString(),
            FirstName = client.FirstName,
            LastName = client.LastName,
            BirthDate = client.BirthDate
        };
    }
}