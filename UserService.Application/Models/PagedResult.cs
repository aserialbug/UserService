namespace UserService.Application.Models;

public record PagedResult<T>(T[] Values, int? Next = null)
{
    public int Count => Values.Length;
}