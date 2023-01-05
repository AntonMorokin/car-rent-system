using Common.Initialization.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Rides.Persistence.Listeners;
using Rides.WebApi.Services;

namespace Rides.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddEnvironmentVariables("RIDES_");

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Services.AddFromInitializers(builder.Configuration,
                new Persistence.ServiceInitializer(),
                new Rides.Services.ServiceInitializer(),
                new Messaging.ServiceInitializer());

            builder.Services.AddSingleton<IHostedService>(p =>
            {
                var mongoClient = p.GetRequiredService<IMongoClient>();
                var ridesListener = ChangeStreamListenerFactory.CreateRidesListener(mongoClient);
                return new ChangeStreamListenerBackgroundService(ridesListener);
            });
            
            builder.Services.AddSingleton<IHostedService>(p =>
            {
                var mongoClient = p.GetRequiredService<IMongoClient>();
                var ridesListener = ChangeStreamListenerFactory.CreateCarsListener(mongoClient);
                return new ChangeStreamListenerBackgroundService(ridesListener);
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllers();

            app.Run();
        }
    }
}