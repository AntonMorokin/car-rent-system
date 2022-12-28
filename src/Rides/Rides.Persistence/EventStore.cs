using MongoDB.Driver;
using Rides.Domain;
using Rides.Persistence.Model;

namespace Rides.Persistence;

public sealed class EventStore<T> : IEventStore<T> where T : Aggregate, new()
{
    private const string DbName = "events";

    private readonly IMongoDatabase _db;
    private readonly string _collectionName;

    public EventStore(IMongoClient mongoClient)
    {
        _db = mongoClient.GetDatabase(DbName);
        _collectionName = DbCollectionMapper.GetCollectionName<T>();
    }

    public Task StoreAsync(T aggregate)
    {
        var collection = _db.GetCollection<EventEnvelope>(_collectionName);

        var events = aggregate.Changes
            .Select(e => EventEnvelope.FromEvent(aggregate, e.Version, e.Event))
            .ToArray();

        return collection.InsertManyAsync(events);
    }

    public async Task<T> LoadAsync(string id)
    {
        var collection = _db.GetCollection<EventEnvelope>(_collectionName);

        var events = await collection
            .Find(e => e.Meta.AggregateId == id)
            .SortBy(e => e.Meta.AggregateVersion)
            .ToListAsync();

        var aggregate = new T();
        aggregate.Load(events.Select(x => x.Payload));

        return aggregate;
    }

    public async Task<string> GetNextIdAsync()
    {
        var collection = _db.GetCollection<EventEnvelope>(_collectionName);

        var lastId = await collection
            .Find(Builders<EventEnvelope>.Filter.Empty)
            .SortByDescending(e => e.Meta.AggregateId)
            .Project(e => e.Meta.AggregateId)
            .SingleOrDefaultAsync();
        
        var lastNumericId = Convert.ToInt32(lastId);
        var newId = lastNumericId + 1;

        return newId.ToString();
    }

    public async Task<bool> CheckIfAggregateExistsAsync(string id)
    {
        var collection = _db.GetCollection<EventEnvelope>(_collectionName);

        var filter = Builders<EventEnvelope>.Filter
            .Eq(e => e.Meta.AggregateId, id);

        var count = await collection.CountDocumentsAsync(filter);

        return count > 0;
    }
}