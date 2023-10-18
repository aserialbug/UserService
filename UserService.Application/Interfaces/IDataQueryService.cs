using UserService.Application.Models;
using UserService.Domain.Posts;
using UserService.Domain.User;

namespace UserService.Application.Interfaces;

public interface IDataQueryService
{
    Task<User?> FindUser(UserId userId);
    Task<PersonViewModel?> FindPerson(UserId userId);
    Task<IEnumerable<PersonViewModel>> SearchPersons(string firstName, string lastName);
    Task<IEnumerable<string>> FindFriends(UserId userId);
    Task<PostViewModel> FindPost(PostId postId);
    Task<IEnumerable<PostViewModel>> GetPosts(UserId userId);
}