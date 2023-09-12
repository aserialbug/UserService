using Microsoft.Extensions.Options;
using Npgsql;

namespace UserService.Infrastructure.Context;

internal class PostgreSqlContext : IAsyncDisposable
{
    private readonly NpgsqlDataSource _dataSource;
    public NpgsqlDataSource DataSource => _dataSource;

    public PostgreSqlContext(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async ValueTask DisposeAsync()
    {
        await _dataSource.DisposeAsync();
    }
    
    public record PostgreSqlContextSettings
    {
        public const string SectionName = "PostgreSqlContext";
        public string ConnectionString { get; init; }
    }
}