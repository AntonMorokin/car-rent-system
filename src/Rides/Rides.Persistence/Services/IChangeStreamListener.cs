namespace Rides.Persistence.Services;

public interface IChangeStreamListener
{
    Task ListenAsync(CancellationToken cts);
}