using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CommunityToolkit.WinUI.Collections;
namespace CollectionManager.WinUI.Utilities;

public class IncrementalSourceFromWebSite(SiteManager siteManager, string query) : IIncrementalSource<GamePageDTO>
{
    public Task<IEnumerable<GamePageDTO>> GetPagedItemsAsync(int pageIndex, int pageSize,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(siteManager.SearchAsync(query, cancellationToken)
            .ToBlockingEnumerable(cancellationToken));
    }
}
