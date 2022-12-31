using Rides.Domain.Aggregates;

namespace Rides.Persistence;

public interface IEventStore<T> where T : Aggregate, new()
{
    Task StoreAsync(T aggregate);

    Task<T> LoadAsync(string id);

    Task<string> GetNextIdAsync();

    Task<bool> CheckIfAggregateExistsAsync(string id);
}