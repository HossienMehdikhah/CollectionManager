using CollectionManager.Core.Contracts.Services;
using CollectionManager.Core.Models;
using Flurl.Http;

namespace CollectionManager.Core.Managers;

public class SiteManager(IGameSiteCrawler _gameSiteCrawler)
{
    public async Task<IEnumerable<GamePageDTO>> GetFeedFromGalleryPage()
    {
        try
        {
            var htmlDocument = await _gameSiteCrawler.CollectionPageURL.GetStringAsync(); ;
            var gamePage = await _gameSiteCrawler.GetGamePagesLinkAsync(htmlDocument);
            //FilterMarkedGame(ref gamePage);
            //DefineGameType(gamePage);
            return gamePage;
        }
        catch (HttpRequestException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    //public async Task<GamePageDto?> GetGameLink(string gameName)
    //{
    //    try
    //    {
    //        var searchQuery = new Uri(string.Format(SiteSearchUrl, gameName).Replace(" ", "+"));
    //        HtmlWeb htmlWeb = new();
    //        var htmlDocument = await htmlWeb.LoadFromWebAsync(searchQuery.ToString());


    //        var gamePageList = _gameSiteCrawler.GetGamePageLink(htmlDocument);
    //        var result = gamePageList.FirstOrDefault(x => Regex.IsMatch(x.Name, $"\\b{gameName}\\b", RegexOptions.IgnoreCase));
    //        if (result != null)
    //        {
    //            var game = _context.GameSet!.FirstOrDefault(x => x.Name == gameName);
    //            if (game != null)
    //                result.MarkedType = game.MarkedType;
    //        }
    //        return result;
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogDebug($"Message:{ex.Message}\nGameName:{gameName}\nStack:{ex.StackTrace}");
    //        return null;
    //    }
    //}
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
