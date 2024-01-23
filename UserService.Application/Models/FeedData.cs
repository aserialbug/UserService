using System.Net.Security;

namespace UserService.Application.Models;

public record FeedData
{
    public string PostId { get; init; }
    public string AuthorId { get; init; }
}