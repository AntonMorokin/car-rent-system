using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Rides.Persistence.Listeners;

namespace Rides.WebApi.Services;

public sealed class ChangeStreamListenerBackgroundService : BackgroundService
{
    private readonly IChangeStreamListener _listener;

    public ChangeStreamListenerBackgroundService(IChangeStreamListener listener)
    {
        _listener = listener;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return _listener.ListenAsync();
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return _listener.StopAsync();
    }
}