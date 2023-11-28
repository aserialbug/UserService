namespace UserService.Application.Models;

public record PagedResult<T>(T[] Values, string? Next)
{
    public int Count => Values.Length;
}