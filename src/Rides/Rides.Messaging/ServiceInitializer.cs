using Common.Initialization;
using Core.Messaging;
using Core.Messaging.Handlers;
using Core.Messaging.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rides.Messaging.Handlers;
using Rides.Messaging.Services;

namespace Rides.Messaging;

public sealed class ServiceInitializer : IServiceInitializer
{
    public void Register(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var kafkaAddress = configuration["Kafka:Address"]
                           ?? throw new InvalidOperationException("In configuration there is no Kafka address");

        serviceCollection.AddSingleton<IMessageProducer>(p =>
        {
            var logger = p.GetRequiredService<ILogger<MessageProducer>>();
            return new MessageProducer(kafkaAddress, logger);
        });

        serviceCollection.AddSingleton<ICarsService, CarsService>();
        serviceCollection.AddSingleton<IClientsService, ClientsService>();

        serviceCollection.AddSingleton<IMessageConsumer>(p =>
        {
            var carsService = p.GetRequiredService<ICarsService>();
            var clientsService = p.GetRequiredService<IClientsService>();
            var loggerFactory = p.GetRequiredService<ILoggerFactory>();

            var handlers = MessageHandlersFactory.CreateCarMessagesHandlers(carsService,
                clientsService,
                loggerFactory);

            var processor = MessageProcessorFactory.Create(handlers, loggerFactory);

            return new MessageConsumer(kafkaAddress,
                Consts.ClientIds.RidesMs,
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