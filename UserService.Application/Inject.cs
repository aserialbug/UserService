﻿using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Models;
using UserService.Application.Services;

namespace UserService.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<LoginService>();
        serviceCollection.AddSingleton<RegisterService>();
        serviceCollection.AddScoped<RequestContext>();
        return serviceCollection;
    }
}