using Core.Messaging.Events;

namespace Core.Messaging.Handlers;

public interface IMessageHandler
{
    string Topic { get; }

    Task HandleAsync(IMessage message);
}