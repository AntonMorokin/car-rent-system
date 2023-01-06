using Rides.Domain.Views;

namespace Rides.Persistence;

public interface IViewStore<TView>
    where TView : ViewBase
{
    Task<TView?> LoadViewByIdOrDefaultAsync(string aggregateId);

    Task StoreViewAsync(TView view);
}