using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CollectionManager.WinUI.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Collections;
using Microsoft.EntityFrameworkCore;
namespace CollectionManager.WinUI.ViewModels;

public partial class ListedViewModel : ObservableObject, IIncrementalSource<GamePageDTO>
{
    public MarkedType MarkedType { get; set; }
    public IncrementalLoadingCollection<ListedViewModel, GamePageDTO> GamePages;
    private readonly SiteManager siteManager;
    private readonly INavigationService navigationService;

    public ListedViewModel(SiteManager siteManager, INavigationService navigationService)
    {
        GamePages = new(this);
        this.siteManager = siteManager;
        this.navigationService = navigationService;
    }
    [RelayCommand]
    public void ItemClick()
    {
        navigationService.NavigateTo(nameof(ContentDisplayViewModel));
    }



    public async Task<IEnumerable<GamePageDTO>> GetPagedItemsAsync(int pageIndex, int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await siteManager.GetGameFromDatabase(MarkedType)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}
