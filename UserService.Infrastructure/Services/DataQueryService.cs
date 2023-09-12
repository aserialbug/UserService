using UserService.Application.Interfaces;
using UserService.Domain.User;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Services;

internal class DataQueryService : IDataQueryService
{
    private readonly PostgreSqlContext _context;
    private readonly PepperService _pepperService;

    public DataQueryService(PostgreSqlContext context, PepperService pepperService)
    {
        _context = context;
        _pepperService = pepperService;
    }

    public async Task<User?> FindUser(UserId userId)
    {
        return await _context.DataSource.FindUserById(userId, _pepperService);
    }
}