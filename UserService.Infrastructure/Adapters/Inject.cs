using Microsoft.Extensions.DependencyInjection;
using UserService.Domain.User;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Adapters;

public static class Inject
{
    public static IServiceCollection AddAdapters(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<DomainEventsDatabaseAdapter>();
        serviceCollection.AddSingleton<IStorageAdapter<User, UserId>, UserDatabaseAdapter>();
        return serviceCollection;
    }
}