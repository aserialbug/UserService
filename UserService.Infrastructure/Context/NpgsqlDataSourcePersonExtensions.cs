﻿using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using UserService.Application.Exceptions;
using UserService.Application.Models;
using UserService.Domain.Person;
using UserService.Domain.User;

namespace UserService.Infrastructure.Context;

internal static class NpgsqlDataSourcePersonExtensions
{
    private const string AddUserCommandSql =
        "insert into persons (id, first_name, last_name, birthday, biography, city) values " +
        "(@userId, @first_name, @last_name, @birthday, @biography, @city)";

    private const string FindPersonByIdSql =
        "select id, first_name, last_name, birthday, biography, city from persons where id = @personId";

    private const string DeletePersonSql = "delete from persons where id = @personId";

    private const string SearchByNameSql =
        "select id, first_name, last_name, birthday, biography, city from persons " +
        "where lower(first_name) like lower(@first) || '%' and lower(last_name) like lower(@last) || '%' order by id";
    
    public static async Task AddPerson(this NpgsqlDataSource dataSource, Person person)
    {
        await using var addUserCommand = dataSource.CreateCommand(AddUserCommandSql);
        addUserCommand.Parameters.AddWithValue("userId", NpgsqlDbType.Uuid, person.Id.ToGuid());
        addUserCommand.Parameters.AddWithValue("first_name", NpgsqlDbType.Text, person.FirstName);
        addUserCommand.Parameters.AddWithValue("last_name", NpgsqlDbType.Text, person.LastName);
        addUserCommand.Parameters.AddWithValue("birthday", NpgsqlDbType.Date, person.Birthday);
        addUserCommand.Parameters.AddWithValue("biography", NpgsqlDbType.Text, person.Biography);
        addUserCommand.Parameters.AddWithValue("city", NpgsqlDbType.Text, person.City);
        await addUserCommand.ExecuteNonQueryAsync();
    }
    
    public static async Task<Person> GetPersonById(this NpgsqlDataSource dataSource, PersonId personId)
    {
        await using var findPersonByIdCommand = dataSource.CreateCommand(FindPersonByIdSql);
        findPersonByIdCommand.Parameters.AddWithValue("personId", NpgsqlDbType.Uuid, personId.ToGuid());
        await using var reader = await findPersonByIdCommand.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            throw new NotFoundException($"Person with id={personId} was not found");

        var id = PersonId.FromGuid(reader.GetGuid(0));
        var firstName = reader.GetString(1);
        var lastName = reader.GetString(2);
        var birthday = reader.GetDateTime(3);
        var biography = reader.GetString(4);
        var city = reader.GetString(5);

        return new Person(id, firstName, lastName, birthday, biography, city);
    }

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

    public static async Task RemovePerson(this NpgsqlDataSource dataSource, PersonId personId)
    {
        await using var deletePersonCommand = dataSource.CreateCommand(DeletePersonSql);
        deletePersonCommand.Parameters.AddWithValue("personId", NpgsqlDbType.Uuid, personId.ToGuid());
        await deletePersonCommand.ExecuteNonQueryAsync();
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