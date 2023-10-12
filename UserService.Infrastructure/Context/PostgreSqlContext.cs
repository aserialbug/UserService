using Microsoft.Extensions.Configuration;
using Npgsql;

namespace UserService.Infrastructure.Context;

internal sealed class PostgreSqlContext : IAsyncDisposable
{
    private readonly NpgsqlMultiHostDataSource _dataSource;

    public NpgsqlDataSource Primary { get; }
    public NpgsqlDataSource Standby { get; }

    public PostgreSqlContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSqlContext");
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
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