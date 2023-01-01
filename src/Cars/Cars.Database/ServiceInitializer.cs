using System;
using Common.Initialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cars.Database;

public sealed class ServiceInitializer : IServiceInitializer
{
    public void Register(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = configuration["Db:ConnectionString"]
                               ?? throw new InvalidOperationException("No connection string configuration");

        serviceCollection.AddSingleton<ICarsRepository>(new CarsRepository(connectionString));
    }
}