using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using UserService.Domain.User;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure.Context;

internal static class NpgsqlDataSourceUserExtensions
{
    private const string AddUserSql = "insert into users (id, login, password) values (@userId, @login, @password)";
    private const string FindUserByIdSql = "select id, login, password from users where id = @userId";
    private const string RemoveUserSql = "delete from users where id = @userId";
    
    public static async Task AddUser(this NpgsqlDataSource dataSource, User user, PepperService? pepperService = default)
    {
        var password = pepperService != null
                ? await pepperService.PepperPassword(user.Password)
                : user.Password.ToString();

        await using var addUserCommand = dataSource.CreateCommand(AddUserSql);
        addUserCommand.Parameters.AddWithValue("userId", NpgsqlDbType.Text, user.Id.ToString());
        addUserCommand.Parameters.AddWithValue("login", NpgsqlDbType.Text, user.Login.ToString());
        addUserCommand.Parameters.AddWithValue("password", NpgsqlDbType.Text, password);
        await addUserCommand.ExecuteNonQueryAsync();
    }
    
    public static async Task<User?> FindUserById(this NpgsqlDataSource dataSource, UserId id, PepperService? pepperService = default)
    {
        await using var findUserByICommand = dataSource.CreateCommand(FindUserByIdSql);
        findUserByICommand.Parameters.AddWithValue("userId", NpgsqlDbType.Text, id.ToString());
        await using var reader = await findUserByICommand.ExecuteReaderAsync();
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
        await using var removeUserCommand = dataSource.CreateCommand(RemoveUserSql);
        removeUserCommand.Parameters.AddWithValue("userId", NpgsqlDbType.Text, userId.ToString());
        await removeUserCommand.ExecuteNonQueryAsync();
    }
}