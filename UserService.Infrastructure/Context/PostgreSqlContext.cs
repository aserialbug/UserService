using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace UserService.Infrastructure.Context;

internal sealed class PostgreSqlContext : IAsyncDisposable
{
    private readonly NpgsqlMultiHostDataSource _dataSource;

    public NpgsqlDataSource Primary { get; }
    public NpgsqlDataSource Standby { get; }

    public PostgreSqlContext(IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        var connectionString = configuration.GetConnectionString("PostgreSqlContext");
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.UseLoggerFactory(loggerFactory);
        _dataSource = dataSourceBuilder.BuildMultiHost();
        Primary = _dataSource.WithTargetSession(TargetSessionAttributes.Primary);
        Standby = _dataSource.WithTargetSession(TargetSessionAttributes.PreferStandby);
    }

    public async ValueTask DisposeAsync()
    {
        await Primary.DisposeAsync();
        await Standby.DisposeAsync();
        await _dataSource.DisposeAsync();
    }
}