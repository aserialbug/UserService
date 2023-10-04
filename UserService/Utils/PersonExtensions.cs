using UserService.Application.Models;
using UserService.Domain.Person;

namespace UserService.Utils;

public static class PersonExtensions
{
    public static UserViewModel ToViewModel(this Person user)
    {
        return new UserViewModel
        {
            First_name = user.FirstName,
            Second_name = user.LastName,
            Birthdate = user.Birthday,
            Biography = user.Biography,
            City = user.City
        };
    }
}