using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.Options;
using UserService.Application.Interfaces;
using UserService.Domain.User;

namespace UserService.Infrastructure.Services;

internal class HashingService : IHashingService
{
    private const int MaxHashLengthBytes = 1024;

    private readonly HashingServiceSettings _serviceSettings;

    public HashingService(IOptions<HashingServiceSettings> serviceSettings)
    {
        _serviceSettings = serviceSettings.Value;
    }

    public async Task<Password> Encrypt(ClearTextPassword password, Salt? salt = null)
    {
        var localSalt = salt ?? GenerateSalt(_serviceSettings.SaltLength);
        using var argon2 = new Argon2id(password.GetBytes().ToArray());
        argon2.Salt = localSalt.GetBytes().ToArray();
        argon2.DegreeOfParallelism = _serviceSettings.DegreeOfParallelism;
        argon2.Iterations = _serviceSettings.Iterations;
        argon2.MemorySize = _serviceSettings.MemorySize;
        var hashBytes = await argon2.GetBytesAsync(
            Math.Max(MaxHashLengthBytes, _serviceSettings.HashLengthBytes));
        var hash = new PasswordHash(hashBytes);

        return new Password(hash, localSalt);
    }

    private Salt GenerateSalt(int saltLength)
    {
        return new Salt(RandomNumberGenerator.GetBytes(saltLength));
    }

    public record HashingServiceSettings
    {
        public const string SectionName = "HashingService";
        
        public int HashLengthBytes { get; init; }
        public int SaltLength { get; init; }
        public int DegreeOfParallelism { get; init; }
        public int Iterations { get; init; }
        public int MemorySize { get; init; }
    }
}