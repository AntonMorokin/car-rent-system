using Common.Initialization.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cars.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddEnvironmentVariables("CARS_");

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Services.AddFromInitializers(builder.Configuration,
                new Database.ServiceInitializer(),
                new Services.ServiceInitializer(),
                new Messaging.ServiceInitializer());

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