namespace UserService.Infrastructure.Models;

public record MigrationDefinition
{
    public int Order { get; init; }
    public string Description { get; init; }
    public string Body { get; init; }
}