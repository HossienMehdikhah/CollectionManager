using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CollectionManager.WinUI.Contracts;
using CollectionManager.WinUI.Singleton;
using CollectionManager.WinUI.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.Collections;
using Microsoft.UI.Xaml;
namespace CollectionManager.WinUI.ViewModels;

public partial class SearchPageViewModel(SiteManager siteManager,
    SearchPageViewModelSingleton singleton) : ObservableObject, INavigationAware
{
    [ObservableProperty]
    private bool progressRingIsActive = false;
    [ObservableProperty]
    private Visibility progressRingVisibility = Visibility.Collapsed;

    public void OnNavigatedFrom()
    {
        
    }

    public void OnNavigatedTo(object parameter)
    {
        
    }

    [RelayCommand]
    private void Search(string searchQuery)
    {
        IIncrementalSource<GamePageDTO> searchResultCollection = new
                IncrementalSourceFromWebSite(siteManager, searchQuery);
        WeakReferenceMessenger.Default.Send(new IncrementalSourceMessage(searchResultCollection));
    }
}
