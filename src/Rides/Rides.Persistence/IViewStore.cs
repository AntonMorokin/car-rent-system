using Rides.Persistence.Views;

namespace Rides.Persistence;

public interface IViewStore<TView>
    where TView : ViewBase, new()
{
    Task<TView?> LoadViewByIdAsync(string aggregateId);

    Task StoreViewAsync(TView view);
}