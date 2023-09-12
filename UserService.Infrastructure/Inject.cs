using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Interfaces;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddSingleton<IPersonRepository, PersonRepository>();
        serviceCollection.AddSingleton<IUserRepository, UserRepository>();
        serviceCollection.AddSingleton<ITokenService, TokenService>();
        serviceCollection.Configure<TokenService.TokenGeneratorServiceSettings>(
            configuration.GetSection(TokenService.TokenGeneratorServiceSettings.SectionName));
        serviceCollection.AddSingleton<IHashingService, HashingService>();
        serviceCollection.Configure<HashingService.HashingServiceSettings>(
            configuration.GetSection(HashingService.HashingServiceSettings.SectionName));
        serviceCollection.AddSingleton<PepperService>();
        serviceCollection.Configure<PepperService.PepperServiceSettings>(
            configuration.GetSection(PepperService.PepperServiceSettings.SectionName));
        serviceCollection.AddSingleton<PostgreSqlContext>();
        serviceCollection.Configure<PostgreSqlContext.PostgreSqlContextSettings>(
            configuration.GetSection(PostgreSqlContext.PostgreSqlContextSettings.SectionName));
        serviceCollection.AddSingleton<JwtSecurityTokenHandler>();
        serviceCollection.AddSingleton<IDataQueryService, DataQueryService>();
        serviceCollection.AddNpgsqlDataSource(configuration.GetConnectionString("PostgreSqlContext"), dataSourceLifetime: ServiceLifetime.Singleton);
        return serviceCollection;
    }
}