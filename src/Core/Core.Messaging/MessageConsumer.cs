using Confluent.Kafka;
using Core.Messaging.Events;
using Core.Messaging.Handlers;
using Core.Messaging.Serialization;
using Microsoft.Extensions.Logging;

namespace Core.Messaging;

public sealed class MessageConsumer : IMessageConsumer, IDisposable
{
    private readonly CancellationTokenSource _cts = new();

    private readonly ConsumerConfig _config;
    private readonly IMessageProcessor _messageProcessor;
    private readonly ILogger<MessageConsumer> _logger;

    private bool _disposed;

    private Task? _loopTask;

    public MessageConsumer(string kafkaAddress,
        string clientGroup,
        IMessageProcessor messageProcessor,
        ILogger<MessageConsumer> logger)
    {
        _config = new ConsumerConfig
        {
            AutoOffsetReset = AutoOffsetReset.Earliest,
            BootstrapServers = kafkaAddress,
            GroupId = clientGroup,
            EnableAutoCommit = true
        };

        _messageProcessor = messageProcessor;
        _logger = logger;
    }

    public void StartListening()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(MessageConsumer));
        }

        _loopTask = Task.Run(ListenLoop);
    }

    private async Task ListenLoop()
    {
        using var consumer = new ConsumerBuilder<string, IMessage>(_config)
            .SetValueDeserializer(Serializer<IMessage>.Single)
            .Build();

        try
        {
            consumer.Subscribe(_messageProcessor.Topics);

            _logger.LogInformation(
                "Started listening for messages from topic {topics}",
                string.Join(", ", _messageProcessor.Topics));

            while (!_cts.IsCancellationRequested)
            {
                var result = consumer.Consume(_cts.Token);

                _logger.LogDebug(
                    "Got message with key={key} from topic {topic}",
                    result.Message.Key,
                    result.Topic);

                await _messageProcessor.HandleAsync(result.Topic, result.Message.Value);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Listening was stopped");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error when listening for messages from {topics}",
                string.Join(", ", _messageProcessor.Topics));

            throw;
        }
        finally
        {
            consumer.Close();
        }
    }

    public void StopListening()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        _cts.Cancel();
        _loopTask?.GetAwaiter().GetResult();
    }
}