using CollectionManager.Core.Contracts.Services;
using CollectionManager.Core.Models;
using Microsoft.Extensions.Options;
namespace CollectionManager.Core.Managers;

public class SiteManager(IGameSiteCrawler _gameSiteCrawler, IOptions<CollectionManagerOption> option)
{
    private uint fetchPostCount = 0;
    private CollectionManagerOption collectionManagerOption= option.Value;
    public async IAsyncEnumerable<GamePageDTO> GetFeedFromGalleryPage()
    {
        await foreach (var item in _gameSiteCrawler.GetFeedAsync(fetchPostCount, collectionManagerOption.MaxAvailablePost))
        {
            fetchPostCount++;
            yield return item;
        }
        //FilterMarkedGame(ref gamePage);
        //DefineGameType(gamePage); 
    }

    public Task<IEnumerable<GamePageDTO>> GetSearchSuggestion(string query)
    {
        return _gameSiteCrawler.GetSearchSuggestionAsync(query);
    }

    public async Task<GamePageDTO> GetSpecificationPageAsync(Uri pageUri)
    {
        return await _gameSiteCrawler.GetPageAsync(pageUri);
    }


    
    //private void FilterMarkedGame(ref IEnumerable<GamePageDto> gamePages)
    //{
    //    var sawGameList = _context.Set<GameSet>().AsQueryable();
    //    List<string> tempGame = gamePages.Select(x => x.Name).ToList();
    //    var SawGameList = sawGameList.Where(x => tempGame.Any(y => y == x.Name)).ToList();
    //    gamePages = gamePages.ExceptBy(SawGameList.Select(x => x.Name), x => x.Name);
    //}
    //private void DefineGameType(IEnumerable<GamePageDto> gamePages)
    //{
    //    foreach (var item in gamePages)
    //    {
    //        var gameGroup = _context.GameSet!.FirstOrDefault(x => x.Name == item.Name);
    //        if (gameGroup == null)
    //            item.MarkedType = MarkedType.New;
    //        else
    //        {
    //            item.MarkedType = gameGroup.MarkedType;
    //        }
    //    }
    //}
}
