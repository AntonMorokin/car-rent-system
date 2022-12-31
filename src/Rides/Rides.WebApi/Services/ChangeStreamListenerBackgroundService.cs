using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Rides.Persistence.Services;

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
        return _listener.ListenAsync(stoppingToken);
    }
}