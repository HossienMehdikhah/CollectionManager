using CollectionManager.Core.Models;

namespace CollectionManager.Core.Contracts.Services;
public interface IGameSiteCrawler
{
    /// <summary>
    /// Use It For Take Query Simulate. Crawler get All Posts But Return That Are not in DataBase
    /// </summary>
    public uint CrawledPostCount { get; }
    IAsyncEnumerable<GamePageDTO> GetFeedAsync(uint skip, uint take, CancellationToken cancellationToken);
    Task<GamePageDTO> GetPageAsync(Uri uri);
    Task<IEnumerable<GamePageDTO>> GetSearchSuggestionAsync(string query);
}
