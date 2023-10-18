namespace UserService.Settings;

internal record AuthenticationSettings
{
    public const string SectionName = "Authentication";
    
    public string PublicKeyFile { get; init; }
    public string Issuer { get; init; }
}