using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Domain.User;

namespace UserService.Application.Services;

public class FeedService
{
    private readonly IDataQueryService _dataQueryService;
    private readonly IFeedCacheService _feedCacheService;

    public FeedService(IDataQueryService dataQueryService, IFeedCacheService feedCacheService)
    {
        _dataQueryService = dataQueryService;
        _feedCacheService = feedCacheService;
    }

    public async Task<IEnumerable<PostViewModel>> GetFeed(UserId userId)
    {
        var feed = await _feedCacheService.GetFeed(userId);
        // var feed = await _dataQueryService.BuildFeed(userId);
        return feed;
    }
}