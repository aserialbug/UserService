using UserService.Domain.Common;
using UserService.Domain.User;

namespace UserService.Domain.Person;

public class Person : Entity<PersonId>
{ 
    public string FirstName { get; }
    public string LastName { get; }
    public DateTime Birthday { get; }
    public string Biography { get; }
    public string City { get; }
    
    public Person(PersonId id, string firstName, string lastName, DateTime birthday, string biography,
        string city) : base(id)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentNullException(nameof(firstName));
        FirstName = firstName;
        
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentNullException(nameof(lastName));
        LastName = lastName;
        Birthday = birthday;
        Biography = biography;
        City = city;
    }
}