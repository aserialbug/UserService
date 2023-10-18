using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserService.Application.Utils;
using UserService.Infrastructure.Services;
using UserService.Settings;

namespace UserService.Utils;

public static class AuthenticationExtensions
{
    private const string KeysBasePath = "Keys";
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var settings = configuration
                    .GetSection(AuthenticationSettings.SectionName)
                    .Get<AuthenticationSettings>()
                    ?? throw new InvalidConfigurationException(
                        $"{AuthenticationSettings.SectionName} configuration section not found");
                
                var publicKey = EncryptionKeyHelper.ReadPublicKey(Path.Combine(KeysBasePath, settings.PublicKeyFile));
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = publicKey,
                    ValidateIssuer = true,
                    ValidIssuer = settings.Issuer,
                    ValidateAudience = false,
                    CryptoProviderFactory = new CryptoProviderFactory()
                    {
                        CacheSignatureProviders = false
                    }
                };
            });
        return serviceCollection;
    }
}