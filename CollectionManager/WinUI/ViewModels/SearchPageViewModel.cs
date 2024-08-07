﻿using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CollectionManager.WinUI.Contracts;
using CollectionManager.WinUI.Singleton;
using CollectionManager.WinUI.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
namespace CollectionManager.WinUI.ViewModels;

public partial class SearchPageViewModel(SiteManager siteManager,
    SearchPageViewModelSingleton singleton) : ObservableObject, INavigationAware
{
    [ObservableProperty]
    private string searchQuery = string.Empty;

    public void OnNavigatedFrom()
    {

    }

    public void OnNavigatedTo(object parameter)
    {
        searchQuery = singleton.QuerySearch;
        ObservableCollection<PostDTO> catchCollection = new(singleton.GamePages);
        WeakReferenceMessenger.Default.Send(new IncrementalSourceMessage(catchCollection));
    }

    [RelayCommand]
    private void Search(string searchQuery)
    {
        WeakReferenceMessenger.Default.Send(new IsLoadingSourceMessage(true));
        singleton.GamePages.Clear();
        singleton.QuerySearch = searchQuery;
        IncrementalSourceFromWebSite searchResultCollection = new(siteManager, searchQuery);
        IncrementalLoadingCollection<IIncrementalSource<PostDTO>, PostDTO> collection = new(searchResultCollection);
        collection.OnEndLoading = () => WeakReferenceMessenger.Default.Send(new IsLoadingSourceMessage(false));
        collection.CollectionChanged += Collection_CollectionChanged;
        WeakReferenceMessenger.Default.Send(new IncrementalSourceMessage(collection));
    }

    private void Collection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        var collection = (IncrementalLoadingCollection<IIncrementalSource<PostDTO>, PostDTO>)sender!;
        singleton.GamePages = [.. collection];
    }
}
