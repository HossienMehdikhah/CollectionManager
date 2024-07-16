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
using System.Collections.ObjectModel;
namespace CollectionManager.WinUI.ViewModels;

public partial class SearchPageViewModel(SiteManager siteManager,
    SearchPageViewModelSingleton singleton) : ObservableObject, INavigationAware
{
    [ObservableProperty]
    private bool progressRingIsActive = false;
    [ObservableProperty]
    private Visibility progressRingVisibility = Visibility.Collapsed;
    [ObservableProperty]
    private string searchQuery = string.Empty;

    public void OnNavigatedFrom()
    {

    }

    public void OnNavigatedTo(object parameter)
    {
        searchQuery = singleton.QuerySearch;
        ObservableCollection<GamePageDTO> catchCollection = new(singleton.GamePages);
        WeakReferenceMessenger.Default.Send(new IncrementalSourceMessage(catchCollection));
    }

    [RelayCommand]
    private void Search(string searchQuery)
    {
        WeakReferenceMessenger.Default.Send(new IsLoadingSourceMessage(true));
        singleton.GamePages.Clear();
        singleton.QuerySearch = searchQuery;
        IncrementalSourceFromWebSite searchResultCollection = new(siteManager, searchQuery);
        IncrementalLoadingCollection<IIncrementalSource<GamePageDTO>, GamePageDTO> collection = new(searchResultCollection);
        collection.OnEndLoading = () => WeakReferenceMessenger.Default.Send(new IsLoadingSourceMessage(false));
        collection.CollectionChanged += Collection_CollectionChanged;
        WeakReferenceMessenger.Default.Send(new IncrementalSourceMessage(collection));
    }

    private void Collection_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        var collection = (IncrementalLoadingCollection<IIncrementalSource<GamePageDTO>, GamePageDTO>)sender!;
        singleton.GamePages = [.. collection];
    }
}
