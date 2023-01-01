namespace Rides.Persistence.Listeners;

public interface IChangeStreamListener
{
    Task ListenAsync(CancellationToken cts);
}