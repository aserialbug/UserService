using Microsoft.Extensions.Options;
using StackExchange.Redis;
using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Domain.User;

namespace UserService.Infrastructure.Services;

public class FeedCacheService : IFeedCacheService
{
    private const int MinFeedLength = 10;
    
    private readonly ConnectionMultiplexer _redis;
    private readonly SerializationService _serializationService;
    private readonly long _maxUserFeedLength;
    
    public FeedCacheService(IOptions<FeedCacheServiceSettings> options, SerializationService serializationService)
    {
        _serializationService = serializationService;
        _redis = ConnectionMultiplexer.Connect(options.Value.ConnectionString);
        _maxUserFeedLength = Math.Max(options.Value.MaxUserFeedLength, MinFeedLength); 
    }
    
    public async Task<IEnumerable<PostViewModel>> GetFeed(UserId userId, int count, string page)
    {
        var db = _redis.GetDatabase();
        var feed = await db.ListRangeAsync(new RedisKey(userId.ToString()));

        return feed
            .Where(p => !p.IsNullOrEmpty)
            .Select(p => _serializationService.Deserialize<PostViewModel>(p.ToString()) ?? throw new InvalidOperationException())
            .ToArray();
    }

    public async Task AddPost(IEnumerable<UserId> users, PostViewModel postViewModel)
    {
        var value = new RedisValue(_serializationService.Serialize(postViewModel));
        var db = _redis.GetDatabase();
        await Task.WhenAll(users
            .Select(async u =>
            {
                var key = new RedisKey(u.ToString());
                await db.ListRightPushAsync(key, value);
                await db.ListTrimAsync(key, 0, _maxUserFeedLength - 1);
            }));
    }

    public async Task CacheFeed(UserId userId, IEnumerable<PostViewModel> posts)
    {
        var db = _redis.GetDatabase();
        var key = new RedisKey(userId.ToString());
        await db.KeyDeleteAsync(key);
        foreach (var postViewModel in posts)
        {
            await db.ListRightPushAsync(key, _serializationService.Serialize(postViewModel));
        }
    }

    public class FeedCacheServiceSettings
    {
        public const string SectionName = "FeedCacheService";
        
        public string ConnectionString { get; init; }
        public int MaxUserFeedLength { get; init; }
    }
}