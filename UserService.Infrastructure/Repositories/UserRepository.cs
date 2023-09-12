using UserService.Application.Exceptions;
using UserService.Application.Interfaces;
using UserService.Domain.User;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly PepperService _pepperService;
    private readonly PostgreSqlContext _context;

    public UserRepository(PepperService pepperService, PostgreSqlContext context)
    {
        _pepperService = pepperService;
        _context = context;
    }

    public Task<User> this[UserId id] => GetById(id);

    private async Task<User> GetById(UserId id)
    {
        return await _context.DataSource.FindUserById(id, _pepperService)
            ?? throw new NotFoundException($"User with id={id} was not found");
    }

    public async Task Add(User entity)
    {
        await _context.DataSource.AddUser(entity, _pepperService);
    }

    public async Task Remove(User entity)
    {
        await _context.DataSource.RemoveUser(entity.Id);
    }
}