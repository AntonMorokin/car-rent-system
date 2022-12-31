using MongoDB.Bson;
using MongoDB.Driver;
using Rides.Domain.Aggregates;
using Rides.Persistence.Events;

namespace Rides.Persistence;

public sealed class EventStore<T> : IEventStore<T> where T : Aggregate, new()
{
    private readonly IMongoCollection<EventEnvelope> _events;
    private readonly IMongoCollection<AggregateVersion> _versions;
    private readonly string _aggregateName;

    public EventStore(IMongoClient mongoClient)
    {
        var db = mongoClient.GetDatabase(Consts.WriteDbName);

        var eventsCollectionName = DbNamesMapper.GetWriteCollectionName<T>();
        _events = db.GetCollection<EventEnvelope>(eventsCollectionName);

        _aggregateName = DbNamesMapper.GetAggregateName<T>();
        _versions = db.GetCollection<AggregateVersion>("aggregate-versions");
    }

    public async Task<string> GetNextIdAsync()
    {
        var lastId = await _events
            .Find(Builders<EventEnvelope>.Filter.Empty)
            .SortByDescending(e => e.Meta.AggregateId)
            .Project(e => e.Meta.AggregateId)
            .FirstOrDefaultAsync();

        var lastNumericId = Convert.ToInt32(lastId);
        var newId = lastNumericId + 1;

        return newId.ToString();
    }

    public async Task<bool> CheckIfAggregateExistsAsync(string id)
    {
        var filter = Builders<EventEnvelope>.Filter
            .Eq(e => e.Meta.AggregateId, id);

        var count = await _events.CountDocumentsAsync(filter);

        return count > 0;
    }

    public async Task StoreAsync(T aggregate)
    {
        var events = aggregate.Changes
            .Select(e => EventEnvelope.FromEvent(aggregate, e.Version, e.Event))
            .ToArray();

        await StoreNewVersionAsync(aggregate.Id, aggregate.InitialVersion, aggregate.Version);
        await StoreEventsAsync(events);
    }

    private async Task StoreNewVersionAsync(string aggregateId, string aggregateInitialVersion, string aggregateVersion)
    {
        // optimistic concurrency for multi-instance interaction with the same aggregate
        var getCurrentVersionRequest = _versions
            .Find(v => v.AggregateName == _aggregateName && v.AggregateId == aggregateId)
            .Project(v => v.Version);

        var currentVersion = await getCurrentVersionRequest.SingleOrDefaultAsync();
        var currentVersionExists = !string.IsNullOrEmpty(currentVersion);

        // use simple double check
        if (currentVersionExists
            && currentVersion != aggregateInitialVersion)
        {
            throw new InvalidOperationException(
                $"The aggregate {_aggregateName} with Id={aggregateId} was changed." +
                $" Current saved version is {currentVersion} when initial version was {aggregateInitialVersion}"
            );
        }

        var filterBuilder = Builders<AggregateVersion>.Filter;
        var filter = filterBuilder.Eq(v => v.AggregateName, _aggregateName)
                     & filterBuilder.Eq(v => v.AggregateId, aggregateId);

        var update = Builders<AggregateVersion>.Update
            .Set(v => v.Version, aggregateVersion)
            .SetOnInsert(v => v.Id, ObjectId.GenerateNewId().ToString());

        await _versions.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

        var newCurrentVersion = await getCurrentVersionRequest.SingleAsync();
        if (newCurrentVersion != aggregateVersion)
        {
            throw new InvalidOperationException(
                $"The aggregate {_aggregateName} with Id={aggregateId} was changed." +
                $" Current saved version is {newCurrentVersion} when initial version was {aggregateInitialVersion}"
            );
        }

        // now we are sure that our aggregateVersion is saved
        // so no one can store their events with the same version 
    }

    private Task StoreEventsAsync(EventEnvelope[] events)
    {
        return _events.InsertManyAsync(events);
    }

    public async Task<T> LoadAsync(string id)
    {
        var events = await _events
            .Find(e => e.Meta.AggregateId == id)
            .SortBy(e => e.Meta.AggregateVersion)
            .ToListAsync();

        var aggregate = new T();
        aggregate.Load(events.Select(x => x.Payload));

        return aggregate;
    }
}