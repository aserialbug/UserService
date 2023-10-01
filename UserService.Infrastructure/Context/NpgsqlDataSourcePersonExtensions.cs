using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using UserService.Application.Models;
using UserService.Domain.Person;
using UserService.Domain.User;

namespace UserService.Infrastructure.Context;

internal static class NpgsqlDataSourcePersonExtensions
{
    public static async Task AddPerson(this NpgsqlDataSource dataSource, Person person)
    {
        await using var addUserCommand = dataSource.CreateCommand("insert into persons (id, first_name, last_name, age, birthday, biography, city) values (@userId, @first_name, @last_name, @age, @birthday, @biography, @city)");
        addUserCommand.Parameters.AddWithValue("userId", NpgsqlDbType.Text, person.Id.ToString());
        addUserCommand.Parameters.AddWithValue("first_name", NpgsqlDbType.Text, person.FirstName);
        addUserCommand.Parameters.AddWithValue("last_name", NpgsqlDbType.Text, person.LastName);
        addUserCommand.Parameters.AddWithValue("age", NpgsqlDbType.Integer, person.Age);
        addUserCommand.Parameters.AddWithValue("birthday", NpgsqlDbType.Date, person.Birthday);
        addUserCommand.Parameters.AddWithValue("biography", NpgsqlDbType.Text, person.Biography);
        addUserCommand.Parameters.AddWithValue("city", NpgsqlDbType.Text, person.City);
        await addUserCommand.ExecuteNonQueryAsync();
    }
    
    public static async Task<Person?> FindPersonById(this NpgsqlDataSource dataSource, UserId userId)
    {
        await using var getByIdCommand = dataSource.CreateCommand("select id, first_name, last_name, age, birthday, biography, city from persons where id = @userId");
        getByIdCommand.Parameters.AddWithValue("userId", NpgsqlDbType.Text, userId.ToString());
        await using var reader = await getByIdCommand.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        var id = UserId.Parse(reader.GetString(0));
        var firstName = reader.GetString(1);
        var lastName = reader.GetString(2);
        var age = reader.GetInt32(3);
        var birthday = reader.GetDateTime(4);
        var biography = reader.GetString(5);
        var city = reader.GetString(6);

        return new Person(id, firstName, lastName, age, birthday, biography, city);
    }
    
    public static async Task RemovePerson(this NpgsqlDataSource dataSource, UserId userId)
    {
        await using var deleteCommand = dataSource.CreateCommand("delete from persons where id = @userId");
        deleteCommand.Parameters.AddWithValue("userId", NpgsqlDbType.Text, userId.ToString());
        await deleteCommand.ExecuteNonQueryAsync();
    }
    
    public static async Task<UserViewModel[]> SearchByName(this NpgsqlDataSource dataSource, string firstNameQuery, string lastNameQuery)
    {
        await using var searchByNameCommand = dataSource.CreateCommand("select first_name, last_name, age, birthday, biography, city from persons where first_name like @first and last_name like @last order by id");
        searchByNameCommand.Parameters.AddWithValue("first", NpgsqlDbType.Text, firstNameQuery);
        searchByNameCommand.Parameters.AddWithValue("last", NpgsqlDbType.Text, lastNameQuery);
        await using var reader = await searchByNameCommand.ExecuteReaderAsync();
        var result = new List<UserViewModel>();
        while (await reader.ReadAsync())
        {
            result.Add(new UserViewModel
            {
                First_name = reader.GetString(0),
                Second_name = reader.GetString(1),
                Age = reader.GetInt32(2),
                Birthdate = reader.GetDateTime(3),
                Biography = reader.GetString(4),
                City = reader.GetString(5)
            });
        }

        return result.ToArray();
    }
}