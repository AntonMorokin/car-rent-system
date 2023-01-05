namespace Core.Messaging;

public interface IMessageConsumer
{
    void StartListening();

    void StopListening();
}