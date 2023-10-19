using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Models;
using UserService.Application.Services;

namespace UserService.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<LoginService>();
        serviceCollection.AddSingleton<RegisterService>();
        serviceCollection.AddSingleton<FriendsService>();
        serviceCollection.AddSingleton<PostsService>();
        serviceCollection.AddScoped<RequestContext>();
        return serviceCollection;
    }
}