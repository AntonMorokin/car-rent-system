using MongoDB.Driver;

namespace Rides.Persistence.Listeners;

public static class ChangeStreamListenerFactory
{
    public static IChangeStreamListener CreateRidesListener(IMongoClient mongoClient)
        => new EventChangesListener<Domain.Aggregates.Ride, Views.Ride>(mongoClient);

    public static IChangeStreamListener CreateCarsListener(IMongoClient mongoClient)
        => new EventChangesListener<Domain.Aggregates.Car, Views.Car>(mongoClient);
}