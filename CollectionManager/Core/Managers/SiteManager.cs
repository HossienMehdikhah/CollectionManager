using CollectionManager.Core.Contracts.Services;
using CollectionManager.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
namespace CollectionManager.Core.Managers;

public class SiteManager(IGameSiteCrawler _gameSiteCrawler, IOptions<CollectionManagerOption> option,
    Context context)
{
    private uint maxAvailablePost = 0;
    public uint FetchPostCount { get; private set; } = 0;
    
    public async IAsyncEnumerable<GamePageDTO> GetFeedFromGalleryPage
        ([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (maxAvailablePost <= option.Value.MaxAvailablePost)
        {
            var posts = await _gameSiteCrawler.GetPostsAsync(FetchPostCount,
                option.Value.MaxAvailablePost,
                cancellationToken);
            FetchPostCount += (uint)posts.Count();
            posts = FilterMarkedGame(posts);

            await foreach (var item in _gameSiteCrawler.GetGamePagesAsync(posts,
                cancellationToken))
            {
                await DefineGameType(item);
                if (item.MarkedType == MarkedType.New)
                {
                    maxAvailablePost++;
                    yield return item;
                }
            }
        }
        maxAvailablePost = 0;
    }
    public IAsyncEnumerable<PostDTO> SearchAsync(string query, CancellationToken cancellationToken = default)
    { 
        return _gameSiteCrawler.SearchAsync(query, cancellationToken);
    }
    public async Task<GamePageDTO> GetSpecificationPageAsync(Uri pageUri)
    {
        var gamePage = await _gameSiteCrawler.GetPageAsync(pageUri);
        await DefineGameType(gamePage);
        return gamePage;
    }
    public async Task AddToUpdateCollection(GamePageDTO gamePageDTO)
    {
        gamePageDTO.MarkedType = MarkedType.Update;
        await AddToCollection(gamePageDTO, MarkedType.Update);
        await context.SaveChangesAsync();
    }
    public async Task AddToIntrestingCollection(GamePageDTO gamePageDTO)
    {
        gamePageDTO.MarkedType = MarkedType.Intresting;
        await AddToCollection(gamePageDTO, MarkedType.Intresting);
        await context.SaveChangesAsync();
    }
    public async Task AddToUnintrestingCollection(GamePageDTO gamePageDTO)
    {
        gamePageDTO.MarkedType = MarkedType.Unintresting;
        await AddToCollection(gamePageDTO, MarkedType.Unintresting);
        await context.SaveChangesAsync();
    }
    public async Task AddToEarlyAccessCollection(GamePageDTO gamePageDTO)
    {
        gamePageDTO.MarkedType = MarkedType.EarlyAccess;
        await AddToCollection(gamePageDTO, MarkedType.EarlyAccess);
        await context.SaveChangesAsync();
    }
    public async Task AddToTesedCollection(GamePageDTO gamePageDTO)
    {
        gamePageDTO.MarkedType = MarkedType.Tested;
        await AddToCollection(gamePageDTO, MarkedType.Tested);
        await context.SaveChangesAsync();
    }
    public IQueryable<GamePageDTO> GetGameFromDatabase(MarkedType markedType)
    {
        return context.Games.AsQueryable().Where(x => x.MarkedType == markedType)
            .Select(x=> new GamePageDTO 
            { 
                Name = x.Name,
                URL = x.Uri,
                MarkedType = x.MarkedType,
                Thumbnail = x.Thumbnail,
            });
    }



    private async Task AddToCollection(GamePageDTO gamePageDTO, MarkedType type)
    {
        try
        {
            var duplicated = await context.Games
                .SingleOrDefaultAsync(x => x.Name.Equals(DatabaseNameNormalizer(gamePageDTO.Name)));
            if (duplicated is null)
            {
                await context.Games.AddAsync(new GameSet
                {
                    MarkedType = type,
                    Name = DatabaseNameNormalizer(gamePageDTO.Name),
                    Uri = gamePageDTO.URL,
                    Thumbnail = gamePageDTO.Thumbnail,
                });
            }
            else
                context.Games.Entry(duplicated).Entity.MarkedType = type;
        }
        catch
        { }
    }
    private async Task DefineGameType(GamePageDTO gamePage)
    {
        var gameGroup = await context.Games.FirstOrDefaultAsync(x => x.Name.Equals(DatabaseNameNormalizer(gamePage.Name)));
        gamePage.MarkedType = gameGroup is null ? MarkedType.New : gameGroup.MarkedType;
    }
    private static string DatabaseNameNormalizer(string name)
    {
        return name.ToLower();
    }
    private IEnumerable<PostDTO> FilterMarkedGame(IEnumerable<PostDTO> posts)
    {
        var sawGameList = context.Games.AsQueryable();
        var filtredPosts = posts.ExceptBy(sawGameList.Select(x=>x.Name),
            x => DatabaseNameNormalizer(x.Name));
        return filtredPosts;
    }
}