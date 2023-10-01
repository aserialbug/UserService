using UserService.Application.Models;
using UserService.Domain.User;

namespace UserService.Application.Interfaces;

public interface IDataQueryService
{
    Task<User?> FindUser(UserId userId);
    Task<UserViewModel[]> Search(string firstName, string lastName);
}