using MongoDB.Bson;
using MongoDB.Driver;
using Rides.Domain.Aggregates;
using Rides.Persistence.Events;
using Rides.Persistence.Views;

namespace Rides.Persistence.Services;

internal abstract class EventChangesListener<TAgg, TModel> : IChangeStreamListener
    where TAgg : Aggregate
    where TModel : ReadModelBase, new()
{
    private const string TokensCollectionName = "tokens";
    private const int DelayTimeoutInMs = 3_000;

    private readonly IMongoCollection<EventEnvelope> _writeCollection;
    private readonly IMongoCollection<TModel> _readCollection;
    private readonly IMongoCollection<ResumeTokenInfo> _tokensCollection;
    private readonly string _aggregateName;

    protected EventChangesListener(IMongoClient mongoClient)
    {
        var writeCollectionName = DbNamesMapper.GetWriteCollectionName<TAgg>();
        _writeCollection = mongoClient.GetDatabase(Consts.WriteDbName)
            .GetCollection<EventEnvelope>(writeCollectionName);

        var readDb = mongoClient.GetDatabase(Consts.ReadDbName);

        var readCollectionName = DbNamesMapper.GetReadCollectionName<TAgg>();
        _readCollection = readDb.GetCollection<TModel>(readCollectionName);

        _tokensCollection = readDb.GetCollection<ResumeTokenInfo>(TokensCollectionName);

        _aggregateName = DbNamesMapper.GetAggregateName<TAgg>();
    }

    public async Task ListenAsync(CancellationToken cts)
    {
        var options = await GetOptionsAsync();
        var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<EventEnvelope>>()
            .Match(d => d.OperationType == ChangeStreamOperationType.Insert
                        || d.OperationType == ChangeStreamOperationType.Update);

        using var changes = await _writeCollection.WatchAsync(pipeline, options, cts);

        while (!cts.IsCancellationRequested)
        {
            while (!cts.IsCancellationRequested
                   && await changes.MoveNextAsync(cts))
            {
                if (!changes.Current.Any())
                {
                    await Task.Delay(DelayTimeoutInMs, cts);
                }

                foreach (var change in changes.Current)
                {
                    var aggregateId = change.FullDocument.Meta.AggregateId;
                    var aggregateVersion = change.FullDocument.Meta.AggregateVersion;

                    var model = await GetModelById(aggregateId);
                    model.Version = aggregateVersion;

                    var newModel = await UpdateModelAsync(model, change);

                    await SaveModelAsync(newModel);
                }
            }

            await Task.Delay(DelayTimeoutInMs, cts);
        }

        var token = changes.GetResumeToken();
        await SaveResumeTokenAsync(token);

        // TODO understand why client is disconnected before the operation is finished
        await Task.Delay(1_000, CancellationToken.None);
    }

    private async Task<ChangeStreamOptions> GetOptionsAsync()
    {
        var options = new ChangeStreamOptions();
        var lastToken = await GetLastTokenAsync();

        if (lastToken is not null)
        {
            options.ResumeAfter = lastToken;
        }

        return options;
    }

    private async Task<BsonDocument?> GetLastTokenAsync()
    {
        var filter = Builders<ResumeTokenInfo>.Filter.Eq(t => t.AggregateName, _aggregateName);
        var info = await _tokensCollection
            .Find(filter)
            .FirstOrDefaultAsync();

        return info?.Token;
    }

    private async Task<TModel> GetModelById(string aggregateId)
    {
        var model = await _readCollection
            .Find(m => m.AggregateId == aggregateId)
            .SingleOrDefaultAsync();

        return model ?? new TModel
        {
            AggregateId = aggregateId
        };
    }

    protected abstract Task<TModel> UpdateModelAsync(TModel model, ChangeStreamDocument<EventEnvelope> change);

    private Task SaveModelAsync(TModel newModel)
    {
        var filter = Builders<TModel>.Filter.Eq(m => m.AggregateId, newModel.AggregateId);
        return _readCollection.ReplaceOneAsync(filter,
            newModel,
            new ReplaceOptions { IsUpsert = true });
    }

    private Task SaveResumeTokenAsync(BsonDocument resumeToken)
    {
        var filter = Builders<ResumeTokenInfo>.Filter.Eq(t => t.AggregateName, _aggregateName);
        var update = Builders<ResumeTokenInfo>.Update
            .Set(t => t.Token, resumeToken)
            .SetOnInsert(t => t.Id, ObjectId.GenerateNewId().ToString())
            .SetOnInsert(t => t.AggregateName, _aggregateName);

        return _tokensCollection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
    }
}