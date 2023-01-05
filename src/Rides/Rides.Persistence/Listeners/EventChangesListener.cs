using MongoDB.Bson;
using MongoDB.Driver;
using Rides.Domain.Aggregates;
using Rides.Persistence.Events;
using Rides.Persistence.Views;

namespace Rides.Persistence.Listeners;

internal sealed class EventChangesListener<TAgg, TView> : IChangeStreamListener, IDisposable
    where TAgg : Aggregate
    where TView : ViewBase, new()
{
    private const string TokensCollectionName = "tokens";
    private const int DelayTimeoutInMs = 3_000;

    private readonly CancellationTokenSource _cts = new();

    private readonly IMongoCollection<EventEnvelope> _events;
    private readonly IViewStore<TView> _viewStore;
    private readonly IMongoCollection<ResumeTokenInfo> _tokensCollection;
    private readonly string _aggregateName;

    private IChangeStreamCursor<ChangeStreamDocument<EventEnvelope>>? _changeStream;

    public EventChangesListener(IMongoClient mongoClient)
    {
        var writeCollectionName = DbNamesMapper.GetWriteCollectionName<TAgg>();
        _events = mongoClient.GetDatabase(Consts.WriteDbName)
            .GetCollection<EventEnvelope>(writeCollectionName);

        var readDb = mongoClient.GetDatabase(Consts.ReadDbName);
        _tokensCollection = readDb.GetCollection<ResumeTokenInfo>(TokensCollectionName);

        _viewStore = new ViewStore<TView>(mongoClient);

        _aggregateName = DbNamesMapper.GetAggregateName<TAgg>();
    }

    public async Task ListenAsync()
    {
        var options = await GetOptionsAsync();
        var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<EventEnvelope>>()
            .Match(d => d.OperationType == ChangeStreamOperationType.Insert);

        _changeStream = await _events.WatchAsync(pipeline, options, _cts.Token);

        while (!_cts.IsCancellationRequested)
        {
            while (!_cts.IsCancellationRequested
                   && await _changeStream.MoveNextAsync(_cts.Token)
                   && _changeStream.Current.Any())
            {
                foreach (var change in _changeStream.Current)
                {
                    var aggregateId = change.FullDocument.Meta.AggregateId;
                    var aggregateVersion = change.FullDocument.Meta.AggregateVersion;

                    var view = await GetViewByIdAsync(aggregateId);

                    view.When(change.FullDocument.Payload);
                    view.Version = aggregateVersion;

                    await _viewStore.StoreViewAsync(view);
                }
            }

            await Task.Delay(DelayTimeoutInMs, _cts.Token);
        }
    }

    public async Task StopAsync()
    {
        try
        {
            _cts.Cancel();

            if (_changeStream is null)
            {
                return;
            }

            var token = _changeStream.GetResumeToken();
            await SaveResumeTokenAsync(token);
        }
        finally
        {
            _changeStream?.Dispose();
            _changeStream = null;
        }
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

    private async Task<TView> GetViewByIdAsync(string aggregateId)
    {
        var view = await _viewStore.LoadViewByIdAsync(aggregateId);

        return view ?? new TView
        {
            Id = ObjectId.GenerateNewId().ToString(),
            AggregateId = aggregateId
        };
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

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}