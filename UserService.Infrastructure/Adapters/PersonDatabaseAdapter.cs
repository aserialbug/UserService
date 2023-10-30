using NpgsqlTypes;
using UserService.Application.Exceptions;
using UserService.Domain.Person;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Adapters;

public class PersonDatabaseAdapter : IStorageAdapter<Person, PersonId>
{
    private const string AddUserCommandSql =
        "insert into persons (id, first_name, last_name, birthday, biography, city) values " +
        "(@userId, @first_name, @last_name, @birthday, @biography, @city)";

    private const string FindPersonByIdSql =
        "select id, first_name, last_name, birthday, biography, city from persons where id = @personId";

    private const string DeletePersonSql = "delete from persons where id = @personId";
    
        public async Task<Person> GetAsync(PersonId id, DatabaseTransaction transaction)
    {
        await using var findPersonByIdCommand = transaction.CreateCommand(FindPersonByIdSql);
        findPersonByIdCommand.Parameters.AddWithValue("personId", NpgsqlDbType.Uuid, id.ToGuid());
        await using var reader = await findPersonByIdCommand.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            throw new NotFoundException($"Person with id={id} was not found");

        var personId = PersonId.FromGuid(reader.GetGuid(0));
        var firstName = reader.GetString(1);
        var lastName = reader.GetString(2);
        var birthday = reader.GetDateTime(3);
        var biography = reader.GetString(4);
        var city = reader.GetString(5);

        return new Person(personId, firstName, lastName, birthday, biography, city);
    }

    public async Task AddAsync(Person entity, DatabaseTransaction transaction)
    {
        await using var addUserCommand = transaction.CreateCommand(AddUserCommandSql);
        addUserCommand.Parameters.AddWithValue("userId", NpgsqlDbType.Uuid, entity.Id.ToGuid());
        addUserCommand.Parameters.AddWithValue("first_name", NpgsqlDbType.Text, entity.FirstName);
        addUserCommand.Parameters.AddWithValue("last_name", NpgsqlDbType.Text, entity.LastName);
        addUserCommand.Parameters.AddWithValue("birthday", NpgsqlDbType.Date, entity.Birthday);
        addUserCommand.Parameters.AddWithValue("biography", NpgsqlDbType.Text, entity.Biography);
        addUserCommand.Parameters.AddWithValue("city", NpgsqlDbType.Text, entity.City);
        await addUserCommand.ExecuteNonQueryAsync();
    }

    public Task UpdateAsync(Person entity, DatabaseTransaction transaction)
    {
        return Task.FromException(new InvalidOperationException("Updating person entity is currently not supported"));
    }

    public async Task DeleteAsync(PersonId id, DatabaseTransaction transaction)
    {
        await using var deletePersonCommand = transaction.CreateCommand(DeletePersonSql);
        deletePersonCommand.Parameters.AddWithValue("personId", NpgsqlDbType.Uuid, id.ToGuid());
        await deletePersonCommand.ExecuteNonQueryAsync();
    }
}