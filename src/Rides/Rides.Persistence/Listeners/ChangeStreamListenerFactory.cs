using MongoDB.Driver;

namespace Rides.Persistence.Listeners;

public static class ChangeStreamListenerFactory
{
    public static IChangeStreamListener CreateRidesListener(IMongoClient mongoClient)
        => new EventChangesListener<Domain.Aggregates.Ride, Views.Ride>(mongoClient);
}