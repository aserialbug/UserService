namespace UserService.Application.Models;

public record LoginResponse
{
    public string Token { get; init; }
}