using UserService.Application.Models;
using UserService.Domain.User;

namespace UserService.Application.Interfaces;

public interface IFeedCacheService
{
    Task<IEnumerable<PostViewModel>> GetFeed(UserId userId);
    Task AddPost(IEnumerable<UserId> users, PostViewModel postViewModel);
    Task CacheFeed(UserId userId, IEnumerable<PostViewModel> posts);
}