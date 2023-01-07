using Common.Initialization;
using Core.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Clients.Services;

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

        serviceCollection.AddSingleton<IClientsService, ClientsService>();
    }
}