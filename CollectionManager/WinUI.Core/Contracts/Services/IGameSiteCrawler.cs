using CollectionManager.Core.Models;

namespace CollectionManager.Core.Contracts.Services;
public interface IGameSiteCrawler
{
    public string CollectionPageURL
    {
        get;
    }
    Task<IEnumerable<GamePageDTO>> GetGamePagesLinkAsync(string htmlDocument);
    Task<GamePageContentDTO> GetGamePageAsync(string htmlDocument);
}
