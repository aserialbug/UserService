using UserService.Application.Models;
using UserService.Domain.Person;

namespace UserService.Utils;

public static class PersonExtensions
{
    public static PersonViewModel ToViewModel(this Person user)
    {
        return new PersonViewModel
        {
            Id = user.Id.ToString(),
            First_name = user.FirstName,
            Second_name = user.LastName,
            Birthdate = user.Birthday,
            Biography = user.Biography,
            City = user.City
        };
    }
}