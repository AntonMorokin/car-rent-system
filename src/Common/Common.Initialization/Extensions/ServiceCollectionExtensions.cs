using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Initialization.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFromInitializers(this IServiceCollection serviceCollection,
        IConfiguration configuration, params IServiceInitializer[] initializers)
    {
        foreach (var initializer in initializers)
        {
            initializer.Register(serviceCollection, configuration);
        }
        
        return serviceCollection;
    }
}