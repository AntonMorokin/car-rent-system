namespace Rides.Persistence.Listeners;

public interface IChangeStreamListener
{
    Task ListenAsync();

    Task StopAsync();
}