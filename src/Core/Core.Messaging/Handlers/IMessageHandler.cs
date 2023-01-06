using Core.Messaging.Events;

namespace Core.Messaging.Handlers;

public interface IMessageHandler
{
    string HandledTopic { get; }

    Task HandleAsync(IMessage message);
}