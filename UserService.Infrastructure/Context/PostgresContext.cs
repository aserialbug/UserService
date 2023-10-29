using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace UserService.Infrastructure.Context;

public sealed class PostgresContext : IAsyncDisposable
{
    private readonly NpgsqlMultiHostDataSource _dataSource;

    public NpgsqlDataSource Primary { get; }
    public NpgsqlDataSource Standby { get; }

    public PostgresContext(IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        var connectionString = configuration.GetConnectionString("PostgresContext");
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.UseLoggerFactory(loggerFactory);
        _dataSource = dataSourceBuilder.BuildMultiHost();
        Primary = _dataSource.WithTargetSession(TargetSessionAttributes.Primary);
        Standby = _dataSource.WithTargetSession(TargetSessionAttributes.PreferStandby);
    }

    public async Task<DatabaseTransaction> BeginTransactionAsync()
    {
        var connection = await Primary.OpenConnectionAsync();
        var transaction = await connection.BeginTransactionAsync();
        return new DatabaseTransaction(connection, transaction);
    }

    public async ValueTask DisposeAsync()
    {
        await Primary.DisposeAsync();
        await Standby.DisposeAsync();
        await _dataSource.DisposeAsync();
    }
}