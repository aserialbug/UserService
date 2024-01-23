using Microsoft.Extensions.Options;
using StackExchange.Redis;
using UserService.Application.Interfaces;
using UserService.Application.Models;

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
    
    public async Task<string[]> GetFeedDataAsync(string userId, int? offset, int? count)
    {
        var db = _redis.GetDatabase();
        var start = offset ?? 0;
        var stop = count ?? -1;
        var feed = await db.ListRangeAsync(new RedisKey(userId), start, stop);

        return feed.Select(v => v.ToString()).ToArray();
    }

    public async Task AddPostAsync(string user, string postId)
    {
        var userKey = new RedisKey(user);
        var postKey = new RedisKey(postId);
        var postValue = new RedisValue(postId);
        var userValue = new RedisValue(user);
        var db = _redis.GetDatabase();

        var length = await db.ListRightPushAsync(userKey, postValue);
        if (length > _maxUserFeedLength)
        {
            var toRemove =  await db.ListLeftPopAsync(userKey, length - _maxUserFeedLength);
            foreach (var data in toRemove)
            {
                await db.SetRemoveAsync(new RedisKey(data.ToString()), userValue);
            }
            await db.ListTrimAsync(userKey, 0, _maxUserFeedLength - 1);
        }
        
        await db.SetAddAsync(postKey, user);
    }

    public async Task ClearFeedAsync(string userId)
    {
        var db = _redis.GetDatabase();
        var key = new RedisKey(userId);
        var feed = await db.ListRangeAsync(key);
        foreach (var data in feed)
        {
            await db.SetRemoveAsync(new RedisKey(data.ToString()), userId);
        }

        await db.KeyDeleteAsync(key);
    }

    public async Task DeletePostAsync(string postId)
    {
        var db = _redis.GetDatabase();
        var postKey = new RedisKey(postId);
        var users = await db.SetMembersAsync(postKey);
        foreach (var user in users)
        {
            await db.ListRemoveAsync(new RedisKey(user.ToString()), new RedisValue(postId));
        }
        await db.KeyDeleteAsync(postKey);
    }

    public class FeedCacheServiceSettings
    {
        public const string SectionName = "FeedCacheService";
        
        public string ConnectionString { get; init; }
        public int MaxUserFeedLength { get; init; }
    }
}