using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CollectionManager.WinUI.Contracts;
using CollectionManager.WinUI.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.Collections;
using WinUI;
namespace CollectionManager.WinUI.ViewModels;

public partial class MarketGameListedPageViewModel(SiteManager siteManager) : ObservableObject, INavigationAware
{
    public void OnNavigatedFrom()
    {
        
    }

    public void OnNavigatedTo(object parameter)
    {
        var markettype = Enum.Parse<MarkedType>((string)parameter);
        IncrementalSourceFromDbByMarktype databseCollection = new(siteManager, markettype);
        IncrementalLoadingCollection<IIncrementalSource<GamePageDTO>, GamePageDTO> collection = new(databseCollection);
        WeakReferenceMessenger.Default.Send(new IncrementalSourceMessage(collection));
    }
}
