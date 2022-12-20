using System;
using Clients.Database;
using Clients.Services;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Clients.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddEnvironmentVariables("CLIENTS_");

            var connectionString = builder.Configuration["Db:ConnectionString"]
                                   ?? throw new InvalidOperationException("No connection string configuration");

            builder.Services.AddSingleton<IClientsRepository>(new ClientsRepository(connectionString));
            builder.Services.AddSingleton<IClientsService, ClientsService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            SqlMapper.AddTypeHandler(Database.Converters.DateOnlyConverter.Single);

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllers();

            app.Run();
        }
    }
}