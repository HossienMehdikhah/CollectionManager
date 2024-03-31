using CollectionManager.Core.Models;

namespace CollectionManager.Core.Contracts.Services;
public interface IGameSiteCrawler
{
    Task<IEnumerable<PostDTO>> GetPostsAsync(uint skip, uint take,
        CancellationToken cancellationToken);
    IAsyncEnumerable<GamePageDTO> GetGamePagesAsync(IEnumerable<PostDTO> posts,
        CancellationToken cancellationToken);
    Task<GamePageDTO> GetPageAsync(Uri uri);
    Task<IEnumerable<GamePageDTO>> GetSearchSuggestionAsync(string query);
}
