namespace UserService.Application.Models;

public record LoginCommand
{
    public string Id { get; init; }
    public string Password { get; init; }
}