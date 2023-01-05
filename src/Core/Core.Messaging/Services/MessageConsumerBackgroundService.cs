using Microsoft.Extensions.Hosting;

namespace Core.Messaging.Services;

public sealed class MessageConsumerBackgroundService : BackgroundService
{
    private readonly IMessageConsumer _consumer;

    public MessageConsumerBackgroundService(IMessageConsumer consumer)
    {
        _consumer = consumer;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.StartListening();
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.StopListening();
        return Task.CompletedTask;
    }
}