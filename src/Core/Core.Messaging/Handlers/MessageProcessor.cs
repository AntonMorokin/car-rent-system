using Core.Messaging.Events;
using Microsoft.Extensions.Logging;

namespace Core.Messaging.Handlers;

internal sealed class MessageProcessor : IMessageProcessor
{
    private readonly ILookup<string, IMessageHandler> _handlers;
    private readonly ILogger<MessageProcessor> _logger;

    public IReadOnlyCollection<string> Topics { get; }

    public MessageProcessor(IReadOnlyCollection<IMessageHandler> handlers, ILogger<MessageProcessor> logger)
    {
        _handlers = handlers.ToLookup(h => h.HandledTopic);
        Topics = _handlers.Select(x => x.Key).ToArray();
        _logger = logger;
    }

    public async Task HandleAsync(string topic, IMessage message)
    {
        _logger.LogDebug(
            "Got for handling message with key={key} from topic {topic}",
            message.Key,
            topic);

        if (!_handlers.Contains(topic))
        {
            _logger.LogError(
                "The message processor is not configured to handle message from topic={topic}",
                topic);

            return;
        }

        try
        {
            foreach (var handler in _handlers[topic])
            {
                await handler.HandleAsync(message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error when handling message");
            throw;
        }

        _logger.LogDebug(
            "Handled message with key={key} from topic {topic}",
            message.Key,
            topic);
    }
}