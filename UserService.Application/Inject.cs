using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Models;

namespace UserService.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<Services.LoginService>();
        serviceCollection.AddSingleton<Services.PersonService>();
        serviceCollection.AddSingleton<Services.RegisterService>();
        serviceCollection.AddScoped<RequestContext>();
        return serviceCollection;
    }
}