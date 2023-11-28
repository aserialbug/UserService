using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Posts;
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

    public async Task<PagedResult<PostViewModel>> GetPagedFeed(UserId userId, int count, string? page)
    {
        count = Math.Max(1, count);
        var feed = await _feedCacheService.GetFeed(userId, count + 1, page);
        var feedArray = feed.ToArray();
        
        string? nextPage = null;
        if (feedArray.Length > count)
        {
            nextPage = feedArray.Last().PostId;
            feedArray = feedArray.Take(count).ToArray();
        }

        return new PagedResult<PostViewModel>(feedArray, nextPage);
    }

    public async Task RebuildFeed(UserId userId)
    {
        var feed = await _dataQueryService.BuildFeed(userId);
        await _feedCacheService.CacheFeed(userId, feed);
    }

    public async Task AddPostToFeeds(PostId postId, UserId authorId)
    {
        var friends = _dataQueryService.FindFriends(authorId);
        var post = _dataQueryService.FindPost(postId);
        await Task.WhenAll(friends, post);

        await _feedCacheService.AddPost(friends.Result.Select(UserId.Parse).Append(authorId), post.Result);
    }
}