using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CollectionManager.WinUI.Contracts;
using CollectionManager.WinUI.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.Collections;
using WinUI;
namespace CollectionManager.WinUI.ViewModels;

public partial class MarketGameListedPageViewModel : ObservableObject, INavigationAware
{
    public void OnNavigatedFrom()
    {
        
    }

    public void OnNavigatedTo(object parameter)
    {
        var markettype = Enum.Parse<MarkedType>((string)parameter);
        IIncrementalSource<GamePageDTO> databseCollection = new
                IncrementalSourceFromDbByMarktype(App.GetService<SiteManager>(), markettype);
        WeakReferenceMessenger.Default.Send(new IncrementalSourceMessage(databseCollection));
    }
}
