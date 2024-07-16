using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CollectionManager.Core.Utilities;
using CommunityToolkit.WinUI.Collections;
using Microsoft.EntityFrameworkCore;
namespace CollectionManager.WinUI.Utilities;

public class IncrementalSourceFromDbByMarktype(SiteManager siteManager, MarkedType markedType) : IIncrementalSource<PostDTO>
{
    public async Task<IEnumerable<PostDTO>> GetPagedItemsAsync(int pageIndex, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var gamePages = await siteManager.GetGameFromDatabase(markedType)
        .Skip(pageIndex * pageSize)
        .Take(pageSize)
        .ToListAsync(cancellationToken);
        gamePages.ForEach(x => x.Name = x.Name.ToCapital());
        return gamePages.Select(x=>
        new PostDTO
        {
            Name = x.Name,
            URL = x.URL!,
            Thumbnail = x.Thumbnail,
        });
    }
}
