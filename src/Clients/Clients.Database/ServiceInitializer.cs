using Common.Initialization;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Clients.Database;

public sealed class ServiceInitializer : IServiceInitializer
{
    public void Register(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        SqlMapper.AddTypeHandler(Converters.DateOnlyConverter.Single);
            
        var connectionString = configuration["Db:ConnectionString"]
                               ?? throw new InvalidOperationException("No connection string configuration");

        serviceCollection.AddSingleton<IClientsRepository>(new ClientsRepository(connectionString));
    }
}