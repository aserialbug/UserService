using Microsoft.Extensions.DependencyInjection;
using UserService.Domain.Friends;
using UserService.Domain.Person;
using UserService.Domain.Posts;
using UserService.Domain.User;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Adapters;

public static class Inject
{
    public static IServiceCollection AddAdapters(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<DomainEventsDatabaseAdapter>();
        serviceCollection.AddSingleton<IStorageAdapter<User, UserId>, UserDatabaseAdapter>();
        serviceCollection.AddSingleton<IStorageAdapter<Person, PersonId>, PersonDatabaseAdapter>();
        serviceCollection.AddSingleton<IStorageAdapter<Post, PostId>, PostsDatabaseAdapter>();
        serviceCollection.AddSingleton<IStorageAdapter<Friendship, FriendshipId>, FriendshipDatabaseAdapter>();
        return serviceCollection;
    }
}