using Cars.Messaging.Handlers;
using Cars.Services;
using Common.Initialization;
using Core.Messaging;
using Core.Messaging.Handlers;
using Core.Messaging.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Cars.Messaging;

public sealed class ServiceInitializer : IServiceInitializer
{
    public void Register(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var kafkaAddress = configuration["Kafka:Address"]
                           ?? throw new InvalidOperationException(
                               "In configuration there is no Kafka address");

        serviceCollection.AddSingleton<IMessageProducer>(p =>
        {
            var logger = p.GetRequiredService<ILogger<MessageProducer>>();
            return new MessageProducer(kafkaAddress, logger);
        });

        serviceCollection.AddSingleton<IMessageConsumer>(p =>
        {
            var carService = p.GetRequiredService<ICarsService>();
            var loggerFactory = p.GetRequiredService<ILoggerFactory>();

            var handlers = MessageHandlersFactory.Create(carService, loggerFactory);
            var processor = MessageProcessorFactory.Create(handlers, loggerFactory);
            return new MessageConsumer(kafkaAddress,
                Consts.ClientIds.CarsMs,
                processor,
                loggerFactory.CreateLogger<MessageConsumer>());
        });

        serviceCollection.AddSingleton<IHostedService>(p =>
        {
            var consumer = p.GetRequiredService<IMessageConsumer>();
            return new MessageConsumerBackgroundService(consumer);
        });
    }
}