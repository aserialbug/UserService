using UserService.Domain.Common;
using UserService.Domain.User;

namespace UserService.Domain.Person;

public class Person : Entity<PersonId>
{
    public Person(PersonId id, string firstName, string lastName, DateTime birthday, string biography,
        string city) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Birthday = birthday;
        Biography = biography;
        City = city;
    }

    public string FirstName { get; }
    public string LastName { get; }
    public DateTime Birthday { get; }
    public string Biography { get; }
    public string City { get; }
}