using System.Reflection;
using Common.Initialization;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Rides.Services;

public sealed class ServiceInitializer : IServiceInitializer
{
    public void Register(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddSingleton<IRidesService, RidesService>();
        serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());
    }
}