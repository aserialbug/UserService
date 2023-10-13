using UserService.Application.Models;
using UserService.Domain.User;

namespace UserService.Application.Interfaces;

public interface IDataQueryService
{
    Task<User?> FindUser(UserId userId);
    Task<PersonViewModel?> FindPerson(UserId userId);
    Task<PersonViewModel[]> SearchPersons(string firstName, string lastName);
}