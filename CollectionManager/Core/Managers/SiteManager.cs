using CollectionManager.Core.Contracts.Services;
using CollectionManager.Core.Models;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;
namespace CollectionManager.Core.Managers;

public class SiteManager(IGameSiteCrawler _gameSiteCrawler, IOptions<CollectionManagerOption> option, Context context)
{
    private uint fetchPostCount = 0;
    private uint maxAvailablePost = 0;
    private readonly CollectionManagerOption collectionManagerOption = option.Value;
    public async IAsyncEnumerable<GamePageDTO> GetFeedFromGalleryPage()
    {
        while (maxAvailablePost <= collectionManagerOption.MaxAvailablePost)
        {
            await foreach (var item in _gameSiteCrawler.GetFeedAsync(fetchPostCount, collectionManagerOption.MaxAvailablePost))
            {
                fetchPostCount++;
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
    public Task<IEnumerable<GamePageDTO>> GetSearchSuggestion(string query)
    {
        return _gameSiteCrawler.GetSearchSuggestionAsync(query);
    }
    public Task<GamePageDTO> GetSpecificationPageAsync(Uri pageUri)
    {
        return _gameSiteCrawler.GetPageAsync(pageUri);
    }

    public async Task AddToUpdateCollection(GamePageDTO gamePageDTO)
    {
        await AddToCollection(gamePageDTO, MarkedType.Update);
        await context.SaveChangesAsync();
        gamePageDTO.MarkedType = MarkedType.Update;
    }
    public async Task AddToMarkCollection(GamePageDTO gamePageDTO)
    {
        await AddToCollection(gamePageDTO, MarkedType.Marked);
        await context.SaveChangesAsync();
        gamePageDTO.MarkedType = MarkedType.Marked;
    }
    public async Task AddToSeenCollection(GamePageDTO gamePageDTO)
    {
        await AddToCollection(gamePageDTO, MarkedType.Seen);
        await context.SaveChangesAsync();
        gamePageDTO.MarkedType = MarkedType.Seen;
    }
    public async Task AddToEarlyAccessCollection(GamePageDTO gamePageDTO)
    {
        await AddToCollection(gamePageDTO, MarkedType.EarlyAccess);
        await context.SaveChangesAsync();
        gamePageDTO.MarkedType = MarkedType.EarlyAccess;
    }



    private async Task AddToCollection(GamePageDTO gamePageDTO, MarkedType type)
    {
        var isDuplicated = await context.Games.FirstOrDefaultAsync(x => x.Name.Equals(NameNormalizer(gamePageDTO.Name)));

        if (isDuplicated is null)
        {
            await context.Games.AddAsync(new GameSet
            {
                MarkedType = type,
                Name = NameNormalizer(gamePageDTO.Name),
                Uri = gamePageDTO.URL,
                Thumbnail = gamePageDTO.Thumbnail,
            });
        }
        else
            context.Games.Entry(isDuplicated).Entity.MarkedType = type;
    }
    private async Task DefineGameType(GamePageDTO gamePage)
    {
        var gameGroup = await context.Games.FirstOrDefaultAsync(x => x.Name.Equals(NameNormalizer(gamePage.Name)));
        gamePage.MarkedType = gameGroup is null ? MarkedType.New : gameGroup.MarkedType;
    }
    private string NameNormalizer(string name)
    {
        return name.Humanize(LetterCasing.LowerCase);
    }

    //private void FilterMarkedGame(ref IEnumerable<GamePageDto> gamePages)
    //{
    //    var sawGameList = _context.Set<GameSet>().AsQueryable();
    //    List<string> tempGame = gamePages.Select(x => x.Name).ToList();
    //    var SawGameList = sawGameList.Where(x => tempGame.Any(y => y == x.Name)).ToList();
    //    gamePages = gamePages.ExceptBy(SawGameList.Select(x => x.Name), x => x.Name);
    //}
}
