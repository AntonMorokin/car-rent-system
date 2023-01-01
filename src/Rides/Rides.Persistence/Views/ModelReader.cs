namespace Rides.Persistence.Views;

internal sealed class ModelReader<TView, TModel> : IModelReader<TModel>
    where TView : ViewBase<TModel>, new()
{
    private readonly IViewStore<TView> _viewStore;

    public ModelReader(IViewStore<TView> viewStore)
    {
        _viewStore = viewStore;
    }

    public async Task<TModel?> LoadModelByIdOrDefault(string aggregateId)
    {
        var view = await _viewStore.LoadViewByIdAsync(aggregateId);
        return view is null
            ? default
            : view.ConvertToModel();
    }
}