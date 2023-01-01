namespace Rides.Persistence.Views;

public interface IModelReader<T>
{
    Task<T?> LoadModelByIdOrDefault(string aggregateId);
}