using Core.Messaging.Events;

namespace Core.Messaging.Handlers;

public interface IMessageProcessor
{
    IReadOnlyCollection<string> Topics { get; }

    Task HandleAsync(string topic, IMessage message);
}