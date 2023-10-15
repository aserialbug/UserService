using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserService.Application.Interfaces;
using UserService.Application.Utils;
using UserService.Domain.User;

namespace UserService.Infrastructure.Services;

internal class TokenService : ITokenService
{
    private const string KeysBasePath = "Keys";
    private const string TokenIssuer = "UserService";

    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly TokenGeneratorServiceSettings _serviceSettings;

    public TokenService(IOptions<TokenGeneratorServiceSettings> serviceSettings, JwtSecurityTokenHandler tokenHandler)
    {
        _tokenHandler = tokenHandler;
        _serviceSettings = serviceSettings.Value;
    }

    public Task<Token> GetToken(User user)
    {
        var privateKey = EncryptionKeyHelper.ReadPrivateKey(
            Path.Combine(KeysBasePath, _serviceSettings.PrivateKeyFile));
        
        var signingCredentials = new SigningCredentials(
            privateKey,
            SecurityAlgorithms.RsaSha256)
        {
            CryptoProviderFactory = new CryptoProviderFactory()
            {
                CacheSignatureProviders = false
            }
        };
        
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString())
        };
        
        var securityToken = new JwtSecurityToken(
            issuer: TokenIssuer,
            expires: DateTime.Now.AddMinutes(_serviceSettings.TokenLifetimeMinutes),
            claims: claims,
            signingCredentials: signingCredentials);

        return Task.FromResult(
                new Token(_tokenHandler.WriteToken(securityToken))
            );
    }

    public record TokenGeneratorServiceSettings
    {
        public const string SectionName = "TokenGenerator";
        
        public string PrivateKeyFile { get; init; }
        public int TokenLifetimeMinutes { get; init; }
    }
}