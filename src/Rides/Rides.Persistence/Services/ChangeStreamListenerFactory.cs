using MongoDB.Driver;

namespace Rides.Persistence.Services;

public static class ChangeStreamListenerFactory
{
    public static IChangeStreamListener CreateRideListener(IMongoClient mongoClient)
        => new RidesChangesListener(mongoClient);
}