using Npgsql;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Repositories;

internal abstract class BaseRepository
{
    protected NpgsqlDataSource DataSource { get; }
    
    protected BaseRepository(PostgresContext postgreSqlContext)
    {
        DataSource = postgreSqlContext.Primary;
    }
}