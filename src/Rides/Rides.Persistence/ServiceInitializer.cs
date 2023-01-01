using Common.Initialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Rides.Domain.Events;
using Rides.Persistence.Serialization;
using Rides.Persistence.Views;

namespace Rides.Persistence;

public sealed class ServiceInitializer : IServiceInitializer
{
    public void Register(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = configuration["Db:ConnectionString"]
                               ?? throw new InvalidOperationException("No connection string configuration");

        serviceCollection.AddSingleton<IMongoClient>(new MongoClient(connectionString));

        BsonSerializer.RegisterDiscriminatorConvention(
            typeof(DomainEventBase),
            new DomainEventDiscriminatorConvention());

        serviceCollection.AddSingleton(typeof(IEventStore<>), typeof(EventStore<>));
        serviceCollection.AddSingleton(typeof(IViewStore<>), typeof(ViewStore<>));
        serviceCollection.AddSingleton<IModelReader<Domain.Model.Ride>>(p =>
        {
            var viewReader = p.GetRequiredService<IViewStore<Ride>>();
            return new ModelReader<Ride, Domain.Model.Ride>(viewReader);
        });
    }
}