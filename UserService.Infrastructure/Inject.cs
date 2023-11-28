using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Interfaces;
using UserService.Infrastructure.Adapters;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<IPersonRepository, PersonRepository>();
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<IFriendshipRepository, FriendshipRepository>();
        serviceCollection.AddScoped<IPostRepository, PostRepository>();
        serviceCollection.AddSingleton<IFeedCacheService, FeedCacheService>();
        serviceCollection.Configure<FeedCacheService.FeedCacheServiceSettings>(
            configuration.GetSection(FeedCacheService.FeedCacheServiceSettings.SectionName));
        serviceCollection.AddSingleton<ITokenService, TokenService>();
        serviceCollection.Configure<TokenService.TokenGeneratorServiceSettings>(
            configuration.GetSection(TokenService.TokenGeneratorServiceSettings.SectionName));
        serviceCollection.AddSingleton<IHashingService, HashingService>();
        serviceCollection.Configure<HashingService.HashingServiceSettings>(
            configuration.GetSection(HashingService.HashingServiceSettings.SectionName));
        serviceCollection.AddSingleton<PepperService>();
        serviceCollection.AddSingleton<SerializationService>();
        serviceCollection.Configure<PepperService.PepperServiceSettings>(
            configuration.GetSection(PepperService.PepperServiceSettings.SectionName));
        serviceCollection.AddSingleton<PostgresContext>();
        serviceCollection.AddSingleton<JwtSecurityTokenHandler>();
        serviceCollection.AddSingleton<IDataQueryService, DataQueryService>();
        serviceCollection.AddTransient<MigrationsService>();
        serviceCollection.Configure<MigrationsService.MigrationsServiceSettings>(
            configuration.GetSection(MigrationsService.MigrationsServiceSettings.SectionName));
        serviceCollection.AddTransient<MigrationDefinitionsService>();
        serviceCollection.AddHostedService<DomainEventsDispatcherService>();
        serviceCollection.Configure<DomainEventsDispatcherService.DomainEventsDispatcherServiceSettings>(
            configuration.GetSection(DomainEventsDispatcherService.DomainEventsDispatcherServiceSettings.SectionName));
        serviceCollection.AddEntitiesContext();
        return serviceCollection;
    }
}