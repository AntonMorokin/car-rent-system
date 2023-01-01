using System;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Rides.Domain.Events;
using Rides.Persistence;
using Rides.Persistence.Listeners;
using Rides.Persistence.Serialization;
using Rides.Persistence.Views;
using Rides.Services;
using Rides.WebApi.Services;

namespace Rides.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddEnvironmentVariables("RIDES_");

            var connectionString = builder.Configuration["Db:ConnectionString"]
                                   ?? throw new InvalidOperationException("No connection string configuration");

            builder.Services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

            BsonSerializer.RegisterDiscriminatorConvention(
                typeof(DomainEventBase),
                new DomainEventDiscriminatorConvention());

            builder.Services.AddSingleton(typeof(IEventStore<>), typeof(EventStore<>));
            builder.Services.AddSingleton(typeof(IViewStore<>), typeof(ViewStore<>));
            builder.Services.AddSingleton<IModelReader<Domain.Model.Ride>>(p =>
            {
                var viewReader = p.GetRequiredService<IViewStore<Persistence.Views.Ride>>();
                return new ModelReader<Persistence.Views.Ride, Domain.Model.Ride>(viewReader);
            });

            builder.Services.AddSingleton<IRidesReadService, RidesReadService>();
            builder.Services.AddSingleton<IRidesWriteService, RidesWriteService>();

            builder.Services.AddSingleton<IHostedService>(p =>
            {
                var mongoClient = p.GetRequiredService<IMongoClient>();
                var ridesListener = ChangeStreamListenerFactory.CreateRidesListener(mongoClient);
                return new ChangeStreamListenerBackgroundService(ridesListener);
            });

            builder.Services.AddMediatR(typeof(Rides.Services.Queries.GetRideByIdQuery).Assembly);

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