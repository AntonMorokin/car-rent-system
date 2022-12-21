using System;
using Cars.Database;
using Cars.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cars.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddEnvironmentVariables("CARS_");

            var dbConnectionString = builder.Configuration["Db:ConnectionString"]
                                     ?? throw new InvalidOperationException("No connection string configuration");

            builder.Services.AddSingleton<ICarsRepository>(new CarsRepository(dbConnectionString));
            builder.Services.AddSingleton<ICarsService, CarsService>();

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