using CollectionManager.Core.Models;

namespace CollectionManager.Core.Contracts.Services;
public interface IGameSiteCrawler
{
    IAsyncEnumerable<GamePageDTO> GetFeedAsync(uint skip, uint take);
    Task<IEnumerable<string>> GetSearchSuggestionAsync(string query);
}
