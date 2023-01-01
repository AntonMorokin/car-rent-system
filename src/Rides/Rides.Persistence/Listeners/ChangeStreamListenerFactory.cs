using MongoDB.Driver;

namespace Rides.Persistence.Listeners;

public static class ChangeStreamListenerFactory
{
    public static IChangeStreamListener CreateRidesListener(IMongoClient mongoClient)
        => new RidesChangesListener(mongoClient);
}