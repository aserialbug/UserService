using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace UserService.Application.Utils;

public static class EncryptionKeyHelper
{
    private const string RsaPublicKeyHeader = "-----BEGIN RSA PUBLIC KEY-----";
    private const string RsaPublicKeyFooter = "-----END RSA PUBLIC KEY-----";
    private const string RsaPrivateKeyHeader = "-----BEGIN RSA PRIVATE KEY-----";
    private const string RsaPrivateKeyFooter = "-----END RSA PRIVATE KEY-----";
    
    public static RsaSecurityKey ReadPublicKey(string keyFileName)
    {
        var rsa = RSA.Create();
        var key = GetKeyStringAsByteArray(keyFileName);
        rsa.ImportRSAPublicKey(key, out _);
        return new RsaSecurityKey(rsa);
    }
    
    public static RsaSecurityKey ReadPrivateKey(string keyFileName)
    {
        var rsa = RSA.Create();
        var key = GetKeyStringAsByteArray(keyFileName);
        rsa.ImportRSAPrivateKey(key, out _);
        return new RsaSecurityKey(rsa);
    }

    private static byte[] GetKeyStringAsByteArray(string fileName)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"Key file {fileName} was not found");

        StringBuilder? builder = null;
        foreach (var line in File.ReadLines(fileName))
        {
            if (line is RsaPublicKeyHeader or RsaPrivateKeyHeader)
            {
                builder = new StringBuilder();
                continue;
            }

            if (line is RsaPublicKeyFooter or RsaPrivateKeyFooter)
                break;

            builder?.Append(line);
        }

        if (builder == null)
            throw new ArgumentException($"Invalid key file {fileName}");

        return Convert.FromBase64String(builder.ToString());
    }
}