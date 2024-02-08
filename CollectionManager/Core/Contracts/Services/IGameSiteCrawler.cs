using CollectionManager.Core.Models;

namespace CollectionManager.Core.Contracts.Services;
public interface IGameSiteCrawler
{
    IAsyncEnumerable<GamePageDTO> GetFeedAsync(uint skip, uint take);
    Task<GamePageDTO> GetPageAsync(Uri uri);
    Task<IEnumerable<GamePageDTO>> GetSearchSuggestionAsync(string query);
}
