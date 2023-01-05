using Core.Messaging.Events;

namespace Core.Messaging;

public interface IMessageProducer
{
    Task SendAsync(string topic, IMessage message);
}