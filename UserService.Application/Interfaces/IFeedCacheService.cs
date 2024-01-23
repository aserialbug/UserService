using UserService.Application.Models;

namespace UserService.Application.Interfaces;

public interface IFeedCacheService
{
    Task<string[]> GetFeedDataAsync(string userId, int? offset, int? count);
    Task AddPostAsync(string userId, string postId);
    Task ClearFeedAsync(string userId);
    Task DeletePostAsync(string postId);
}