using Confluent.Kafka;
using Core.Messaging.Events;
using Core.Messaging.Serialization;
using Microsoft.Extensions.Logging;

namespace Core.Messaging;

public sealed class MessageProducer : IMessageProducer, IDisposable
{
    private readonly IProducer<string, IMessage> _producer;
    private readonly ILogger<MessageProducer> _logger;
    private bool _disposed;

    public MessageProducer(string kafkaAddress, ILogger<MessageProducer> logger)
    {
        var config = new ProducerConfig
        {
            Acks = Acks.Leader,
            Partitioner = Partitioner.ConsistentRandom,
            LingerMs = 500,
            BootstrapServers = kafkaAddress
        };

        _producer = new ProducerBuilder<string, IMessage>(config)
            .SetValueSerializer(Serializer<IMessage>.Single)
            .Build();

        _logger = logger;
    }

    public async Task SendAsync(string topic, IMessage message)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(MessageProducer));
        }

        _logger.LogDebug("Got message for topic {topic} with key={key}", topic, message.Key);

        var kafkaMessage = new Message<string, IMessage>
        {
            Key = message.Key,
            Value = message
        };

        // Enough for low and mid rates
        await _producer.ProduceAsync(topic, kafkaMessage);

        _logger.LogDebug("Message with key={key} was sent to topic {topic}", message.Key, topic);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        _producer.Flush();
        _producer.Dispose();
    }
}