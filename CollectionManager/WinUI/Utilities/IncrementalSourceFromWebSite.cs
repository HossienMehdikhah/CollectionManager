using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CommunityToolkit.WinUI.Collections;
namespace CollectionManager.WinUI.Utilities;

public class IncrementalSourceFromWebSite(SiteManager siteManager, string query) : IIncrementalSource<PostDTO>
{
    public async Task<IEnumerable<PostDTO>> GetPagedItemsAsync(int pageIndex, int pageSize,
        CancellationToken cancellationToken = default)
    {
        List<PostDTO> gamePageDTOs = [];
        if (pageIndex > 1)
            return gamePageDTOs;

        await foreach (var item in siteManager.SearchAsync(query, cancellationToken))
            gamePageDTOs.Add(item);

        return gamePageDTOs;
    }
}
