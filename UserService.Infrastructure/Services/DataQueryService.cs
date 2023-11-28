using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Npgsql;
using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Person;
using UserService.Domain.Posts;
using UserService.Domain.User;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Services;

internal class DataQueryService : IDataQueryService
{
    private readonly PepperService _pepperService;
    
    private NpgsqlDataSource DataSource { get; }

    public DataQueryService(PostgresContext context, PepperService pepperService)
    {
        DataSource = context.Standby;
        _pepperService = pepperService;
    }

    public async Task<User?> FindUser(UserId userId)
    {
        return await DataSource.FindUserById(userId, _pepperService);
    }

    public async Task<PersonViewModel?> FindPerson(PersonId personId)
    {
        return await DataSource.FindPersonById(personId);
    }

    public async Task<IEnumerable<PersonViewModel>> SearchPersons(string firstName, string lastName)
    {
        return await DataSource.SearchByName(firstName, lastName);
    }

    public async Task<IEnumerable<string>> FindFriends(UserId userId)
    {
        return await DataSource.FindFriends(userId);
    }

    public async Task<PostViewModel?> FindPost(PostId postId)
    {
        return await DataSource.FindPost(postId);
    }

    public async Task<IEnumerable<PostViewModel>> GetPosts(UserId userId)
    {
        return await DataSource.GetUserPosts(userId);
    }

    public async Task<IEnumerable<PostViewModel>> BuildFeed(UserId userId)
    {
        var friends = await DataSource.FindFriends(userId);
        
        return await DataSource.BuildFeed(friends);
    }
}