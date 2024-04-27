using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CollectionManager.Core.Utilities;
using CollectionManager.WinUI.Contracts;
using CollectionManager.WinUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml.Controls;
namespace CollectionManager.WinUI.ViewModels;

public partial class ListedViewModel : ObservableObject, IIncrementalSource<GamePageDTO>
{
    public MarkedType MarkedType { get; set; }
    public IncrementalLoadingCollection<ListedViewModel, GamePageDTO> GamePages { get;  } 
        
    private readonly SiteManager siteManager;
    private readonly INavigationService navigationService;

    public ListedViewModel(SiteManager siteManager, INavigationService navigationService)
    {
        this.siteManager = siteManager;
        this.navigationService = navigationService;
        GamePages = new(this);

    }
    [RelayCommand]
    public void ItemClick(ItemClickEventArgs e)
    {
        var selectedItem = (GamePageDTO)e.ClickedItem;
        navigationService.NavigateTo(typeof(DisplayGameViewModel).FullName!, selectedItem);
    }

    public async Task<IEnumerable<GamePageDTO>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        var gamePages = await siteManager.GetGameFromDatabase(MarkedType)
        .Skip(pageIndex * pageSize)
        .Take(pageSize)
        .ToListAsync(cancellationToken);
        gamePages.ForEach(x => x.Name = x.Name.ToCapital());
        return gamePages;
    }    
}


