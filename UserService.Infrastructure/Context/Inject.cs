using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.Adapters;

namespace UserService.Infrastructure.Context;

public static class Inject
{
    public static IServiceCollection AddEntitiesContext(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ServiceFactory>(provider => provider.GetRequiredService);
        serviceCollection.AddScoped<EntitiesContext>();
        serviceCollection.AddAdapters();
        return serviceCollection;
    }
}