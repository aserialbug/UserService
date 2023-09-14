using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using UserService.Domain.User;

namespace UserService.Infrastructure.Services;

internal class PepperService : IDisposable
{
    private readonly Aes _cryptoService;
    private const string InitializationVectorString = "HK0WAGowSvtsgsU3kXmeeA==";

    public PepperService(IOptions<PepperServiceSettings> serviceSettings)
    {
        _cryptoService = Aes.Create();
        _cryptoService.Key = Convert.FromBase64String(serviceSettings.Value.Pepper);
        _cryptoService.IV = Convert.FromBase64String(InitializationVectorString);
    }

    public async Task<string> PepperPassword(Password password)
    {
        using var memory = new MemoryStream();
        var encryptor = _cryptoService.CreateEncryptor();
        await using var cryptoStream = new CryptoStream(
            memory, 
            encryptor, 
            CryptoStreamMode.Write);
        await using (var writer = new StreamWriter(cryptoStream))
        {
            await writer.WriteAsync(password.ToString());
        }
        return Convert.ToBase64String(memory.ToArray());
    }

    public async Task<Password> ReadPepperedPassword(string password)
    {
        var bytes = Convert.FromBase64String(password);
        using var memory = new MemoryStream(bytes);
        var dEncryptor = _cryptoService.CreateDecryptor();
        await using var cryptoStream = new CryptoStream(
            memory, 
            dEncryptor, 
            CryptoStreamMode.Read);
        
        var result = string.Empty;
        using (var reader = new StreamReader(cryptoStream))
        {
            result = await reader.ReadToEndAsync();
        }

        return Password.Parse(result);
    }
    
    public record PepperServiceSettings
    {
        public const string SectionName = "PepperService";
        public string Pepper { get; init; }
    }

    public void Dispose()
    {
        _cryptoService.Dispose();
    }
}