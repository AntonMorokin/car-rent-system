using MongoDB.Driver;
using Rides.Persistence.Views;

namespace Rides.Persistence;

internal sealed class ViewStore<TView> : IViewStore<TView>
    where TView : ViewBase, new()
{
    private readonly IMongoCollection<TView> _views;

    public ViewStore(IMongoClient mongoClient)
    {
        var readDb = mongoClient.GetDatabase(Consts.ReadDbName);

        var readCollectionName = DbNamesMapper.GetReadCollectionName<TView>();
        _views = readDb.GetCollection<TView>(readCollectionName);
    }

    public async Task<TView?> LoadViewByIdAsync(string aggregateId)
    {
        return await _views
            .Find(m => m.AggregateId == aggregateId)
            .SingleOrDefaultAsync();
    }

    public Task StoreViewAsync(TView view)
    {
        var filter = Builders<TView>.Filter.Eq(m => m.AggregateId, view.AggregateId);
        return _views.ReplaceOneAsync(filter,
            view,
            new ReplaceOptions { IsUpsert = true });
    }
}