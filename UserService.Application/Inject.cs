using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Models;
using UserService.Application.Services;

namespace UserService.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<LoginService>();
        serviceCollection.AddScoped<RegisterService>();
        serviceCollection.AddScoped<FriendsService>();
        serviceCollection.AddScoped<PostsService>();
        serviceCollection.AddScoped<RequestContext>();
        serviceCollection.AddSingleton<SessionManager>();
        return serviceCollection;
    }
}