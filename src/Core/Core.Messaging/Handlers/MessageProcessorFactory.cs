using Microsoft.Extensions.Logging;

namespace Core.Messaging.Handlers;

public static class MessageProcessorFactory
{
    public static IMessageProcessor Create(IReadOnlyCollection<IMessageHandler> handlers, ILoggerFactory loggerFactory)
    {
        return new MessageProcessor(handlers, loggerFactory.CreateLogger<MessageProcessor>());
    }
}