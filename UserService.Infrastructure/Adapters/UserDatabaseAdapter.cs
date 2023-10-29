using NpgsqlTypes;
using UserService.Application.Exceptions;
using UserService.Domain.User;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure.Adapters;

internal class UserDatabaseAdapter : IStorageAdapter<User, UserId>
{
    private const string AddUserSql = "insert into users (id, login, password) values (@userId, @login, @password)";
    private const string FindUserByIdSql = "select id, login, password from users where id = @userId";
    private const string RemoveUserSql = "delete from users where id = @userId";
    
    private readonly PepperService _pepperService;

    public UserDatabaseAdapter(PepperService pepperService)
    {
        _pepperService = pepperService;
    }

    public async Task<User> GetAsync(UserId id, DatabaseTransaction transaction)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        
        await using var findUserByICommand = transaction.CreateCommand(FindUserByIdSql);
        findUserByICommand.Parameters.AddWithValue("userId", NpgsqlDbType.Uuid, id.ToGuid());
        await using var reader = await findUserByICommand.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            throw new NotFoundException($"User with id={id} was not found");

        var userId = UserId.FromGuid(reader.GetGuid(0));
        var login = Login.Parse(reader.GetString(1));
        var stringPassword = reader.GetString(2);
        var password = await _pepperService.ReadPepperedPassword(stringPassword);

        return new User(userId, login, password);
    }

    public async Task AddAsync(User entity, DatabaseTransaction transaction)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));

        var password = await _pepperService.PepperPassword(entity.Password);

        await using var addUserCommand = transaction.CreateCommand(AddUserSql);
        addUserCommand.Parameters.AddWithValue("userId", NpgsqlDbType.Uuid, entity.Id.ToGuid());
        addUserCommand.Parameters.AddWithValue("login", NpgsqlDbType.Text, entity.Login.ToString());
        addUserCommand.Parameters.AddWithValue("password", NpgsqlDbType.Text, password);
        await addUserCommand.ExecuteNonQueryAsync();
    }

    public Task UpdateAsync(User entity, DatabaseTransaction transaction)
    {
        return Task.FromException(new InvalidOperationException("Updating user entity is currently not supported"));
    }

    public async Task DeleteAsync(UserId id, DatabaseTransaction transaction)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));

        await using var removeUserCommand = transaction.CreateCommand(RemoveUserSql);
        removeUserCommand.Parameters.AddWithValue("userId", NpgsqlDbType.Uuid, id.ToGuid());
        await removeUserCommand.ExecuteNonQueryAsync();
    }
}