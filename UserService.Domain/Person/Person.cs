using System;
using UserService.Domain.Common;
using UserService.Domain.User;

namespace UserService.Domain.Person;

public class Person : Entity<UserId>
{
    public string FirstName { get; }
    public string LastName { get; }
    public int Age { get; }
    public DateTime Birthday { get; }
    public string Biography { get; }
    public string City { get; }

    public Person(UserId id, string firstName, string lastName, int age, DateTime birthday, string biography, string city) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Birthday = birthday;
        Biography = biography;
        City = city;
    }
}