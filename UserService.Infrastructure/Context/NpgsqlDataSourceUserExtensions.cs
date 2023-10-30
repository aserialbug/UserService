using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using UserService.Domain.User;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure.Context;

internal static class NpgsqlDataSourceUserExtensions
{
    private const string FindUserByIdSql = "select id, login, password from users where id = @userId";

    public static async Task<User?> FindUserById(this NpgsqlDataSource dataSource, UserId id, PepperService? pepperService = default)
    {
        await using var findUserByICommand = dataSource.CreateCommand(FindUserByIdSql);
        findUserByICommand.Parameters.AddWithValue("userId", NpgsqlDbType.Uuid, id.ToGuid());
        await using var reader = await findUserByICommand.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        var userId = UserId.FromGuid(reader.GetGuid(0));
        var login = Login.Parse(reader.GetString(1));
        var stringPassword = reader.GetString(2);
        var password = pepperService != null
            ? await pepperService.ReadPepperedPassword(stringPassword)
            : Password.Parse(stringPassword);

        return new User(userId, login, password);
    }
}