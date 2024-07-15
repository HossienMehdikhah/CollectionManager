using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CollectionManager.Core.Utilities;
using CommunityToolkit.WinUI.Collections;
using Microsoft.EntityFrameworkCore;
namespace CollectionManager.WinUI.Utilities;

public class IncrementalSourceFromDbByMarktype : IIncrementalSource<GamePageDTO>
{
    private readonly SiteManager siteManager;
    private readonly MarkedType markedType;

    public IncrementalSourceFromDbByMarktype(SiteManager siteManager, MarkedType markedType)
    {
        this.siteManager = siteManager;
        this.markedType = markedType;
    }

    public async Task<IEnumerable<GamePageDTO>> GetPagedItemsAsync(int pageIndex, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var gamePages = await siteManager.GetGameFromDatabase(markedType)
        .Skip(pageIndex * pageSize)
        .Take(pageSize)
        .ToListAsync(cancellationToken);
        gamePages.ForEach(x => x.Name = x.Name.ToCapital());
        return gamePages;
    }
}
