﻿using CollectionManager.Core.Models;

namespace CollectionManager.Core.Contracts.Services;
public interface IGameSiteCrawler
{
    IAsyncEnumerable<PostDTO> GetPostsAsync(uint skip, uint take,
        CancellationToken cancellationToken);
    IAsyncEnumerable<GamePageDTO> GetGamePagesAsync(IEnumerable<PostDTO> posts,
        CancellationToken cancellationToken);
    Task<GamePageDTO> GetPageAsync(Uri uri);
    IAsyncEnumerable<PostDTO> SearchAsync(string query,
        CancellationToken cancellationToken);
}
