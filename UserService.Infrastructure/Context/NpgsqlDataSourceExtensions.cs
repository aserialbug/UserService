using Npgsql;
using NpgsqlTypes;
using UserService.Domain.User;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure.Context;

internal static class NpgsqlDataSourceUserExtensions
{
    public static async Task AddUser(this NpgsqlDataSource dataSource, User user, PepperService? pepperService = default)
    {
        var password = pepperService != null
                ? await pepperService.PepperPassword(user.Password)
                : user.Password.ToString();

        await using var addUserCommand = dataSource.CreateCommand("insert into users (id, login, password) values (@userId, @login, @password)");
        addUserCommand.Parameters.AddWithValue("userId", NpgsqlDbType.Text, user.Id.ToString());
        addUserCommand.Parameters.AddWithValue("login", NpgsqlDbType.Text, user.Login.ToString());
        addUserCommand.Parameters.AddWithValue("password", NpgsqlDbType.Text, password);
        await addUserCommand.ExecuteNonQueryAsync();
    }
    
    public static async Task<User?> FindUserById(this NpgsqlDataSource dataSource, UserId id, PepperService? pepperService = default)
    {
        await using var getByIdCommand = dataSource.CreateCommand("select id, login, password from users where id = @userId");
        getByIdCommand.Parameters.AddWithValue("userId", NpgsqlDbType.Text, id.ToString());
        await using var reader = await getByIdCommand.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        var userId = UserId.Parse(reader.GetString(0));
        var login = Login.Parse(reader.GetString(1));
        var stringPassword = reader.GetString(2);
        var password = pepperService != null
            ? await pepperService.ReadPepperedPassword(stringPassword)
            : Password.Parse(stringPassword);

        return new User(userId, login, password);
    }
    
    public static async Task RemoveUser(this NpgsqlDataSource dataSource, UserId userId)
    {
        await using var deleteCommand = dataSource.CreateCommand("delete from users where id = @userId");
        deleteCommand.Parameters.AddWithValue("userId", NpgsqlDbType.Text, userId.ToString());
        await deleteCommand.ExecuteNonQueryAsync();
    }
}