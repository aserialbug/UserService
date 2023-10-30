using Npgsql;
using NpgsqlTypes;
using UserService.Application.Models;
using UserService.Domain.Person;
using UserService.Domain.User;

namespace UserService.Infrastructure.Context;

internal static class NpgsqlDataSourcePersonExtensions
{
    private const string FindPersonByIdSql =
        "select id, first_name, last_name, birthday, biography, city from persons where id = @personId";

    private const string SearchByNameSql =
        "select id, first_name, last_name, birthday, biography, city from persons " +
        "where lower(first_name) like lower(@first) || '%' and lower(last_name) like lower(@last) || '%' order by id";
    

    public static async Task<PersonViewModel?> FindPersonById(this NpgsqlDataSource dataSource, PersonId personId)
    {
        await using var findPersonByIdCommand = dataSource.CreateCommand(FindPersonByIdSql);
        findPersonByIdCommand.Parameters.AddWithValue("personId", NpgsqlDbType.Uuid, personId.ToGuid());
        await using var reader = await findPersonByIdCommand.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        return new PersonViewModel
        {
            Id = UserId.FromGuid(reader.GetGuid(0)).ToString(),
            First_name = reader.GetString(1),
            Second_name = reader.GetString(2),
            Birthdate = reader.GetDateTime(3),
            Biography = reader.GetString(4),
            City = reader.GetString(5)
        };
    }

    public static async Task<PersonViewModel[]> SearchByName(this NpgsqlDataSource dataSource, string firstNameQuery, string lastNameQuery)
    {
        await using var searchByNameCommand = dataSource.CreateCommand(SearchByNameSql);
        searchByNameCommand.Parameters.AddWithValue("first", NpgsqlDbType.Text, firstNameQuery);
        searchByNameCommand.Parameters.AddWithValue("last", NpgsqlDbType.Text, lastNameQuery);
        await using var reader = await searchByNameCommand.ExecuteReaderAsync();
        var result = new List<PersonViewModel>();
        while (await reader.ReadAsync())
        {
            result.Add(new PersonViewModel
            {
                Id = UserId.FromGuid(reader.GetGuid(0)).ToString(),
                First_name = reader.GetString(1),
                Second_name = reader.GetString(2),
                Birthdate = reader.GetDateTime(3),
                Biography = reader.GetString(4),
                City = reader.GetString(5)
            });
        }

        return result.ToArray();
    }
}