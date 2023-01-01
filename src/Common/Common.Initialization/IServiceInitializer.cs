using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Initialization;

public interface IServiceInitializer
{
    void Register(IServiceCollection serviceCollection, IConfiguration configuration);
}