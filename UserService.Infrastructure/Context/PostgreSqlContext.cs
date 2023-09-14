using Npgsql;

namespace UserService.Infrastructure.Context;

internal class PostgreSqlContext
{
    private readonly NpgsqlDataSource _dataSource;
    public NpgsqlDataSource DataSource => _dataSource;

    public PostgreSqlContext(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
}