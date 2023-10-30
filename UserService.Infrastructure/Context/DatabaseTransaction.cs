using Npgsql;

namespace UserService.Infrastructure.Context;

public sealed class DatabaseTransaction : IAsyncDisposable
{
    private readonly NpgsqlConnection _connection;
    private readonly NpgsqlTransaction _transaction;
    public bool Active { get; private set; } = true;
    
    public DatabaseTransaction(NpgsqlConnection connection, NpgsqlTransaction transaction)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
    }

    public async Task Commit()
    {
        ThrowIfNotActive();
        await _transaction.CommitAsync();
        Deactivate();
    }

    public async Task Rollback()
    {
        ThrowIfNotActive();
        await _transaction.RollbackAsync();
        Deactivate();
    }

    internal NpgsqlCommand CreateCommand(string sql)
    {
        ThrowIfNotActive();
        if (string.IsNullOrWhiteSpace(sql))
            throw new ArgumentNullException(nameof(sql));

        return new NpgsqlCommand(sql, _connection, _transaction);
    }

    internal NpgsqlBatchCommand CreateBatchCommand(string sql)
    {
        ThrowIfNotActive();
        if (string.IsNullOrWhiteSpace(sql))
            throw new ArgumentNullException(nameof(sql));

        return new NpgsqlBatchCommand(sql);
    }

    internal NpgsqlBatch CreateBatch(IEnumerable<NpgsqlBatchCommand> commands)
    {
        ThrowIfNotActive();
        var result = new NpgsqlBatch(_connection, _transaction);
        foreach (var batchCommand in commands)
        {
            result.BatchCommands.Add(batchCommand);
        }

        return result;
    }
    
    public async ValueTask DisposeAsync()
    {
        await _transaction.DisposeAsync();
        await _connection.DisposeAsync();
        Deactivate();
    }

    private void Deactivate()
        => Active = false;

    private void ThrowIfNotActive()
    {
        if (!Active) 
            throw new InvalidOperationException("Transaction is not active");
    }
}