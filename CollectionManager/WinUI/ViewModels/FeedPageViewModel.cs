using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CollectionManager.Core.Utilities;
using CollectionManager.WinUI.Contracts;
using CollectionManager.WinUI.Singleton;
using CollectionManager.WinUI.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Options;
namespace CollectionManager.WinUI.ViewModels;

public partial class FeedPageViewModel(SiteManager siteManager,
    IOptions<CollectionManagerOption> option, 
    FeedPageViewModelSingleton singleton) : ObservableObject, INavigationAware
{
    private int gamePageListIndex = -1;
    private bool isBackgroundWorkerRunning;
    private GamePageDTO CurrentPage
    {
        set
        {
            WeakReferenceMessenger.Default.Send(new CurrentPageSourceMessage(value));
        }
    }
    private readonly CancellationTokenSource CancellationTokenSource = new();
    private bool _isLoading = false;

    public async void OnNavigatedTo(object parameter)
    {
        if (singleton.GamePageList.Count == 0)
            await FetchPostsAndShowFirstItem();
        else
            CurrentPage = singleton.GamePageList[++gamePageListIndex];
    }
    public async void OnNavigatedFrom()
    {
        await CancellationTokenSource.CancelAsync();
        singleton.FetchedPostsCount = singleton.FetchedPostsCount;
    }

    [RelayCommand]
    private async Task NextButtonEvent()
    {
        if (gamePageListIndex + 1 > singleton.GamePageList.Count && isBackgroundWorkerRunning)
        {
            _isLoading = true;
            WeakReferenceMessenger.Default.Send(new IsLoadingSourceMessage(true));
        }
        else
        {
            CurrentPage = singleton.GamePageList[++gamePageListIndex];
            if (!isBackgroundWorkerRunning
                && singleton.GamePageList.Count - (gamePageListIndex + 1) <= option.Value.SearchThreshold)
            {
                isBackgroundWorkerRunning = true;
                await foreach (var gamePage in siteManager.GetFeedFromGalleryPage(CancellationTokenSource.Token))
                {
                    gamePage.Name = gamePage.Name.ToCapital();
                    singleton.GamePageList.Add(gamePage);
                }
                isBackgroundWorkerRunning = false;
            }
        }
    }
    [RelayCommand]
    private void PreviousButtonEvent()
    {
        if (gamePageListIndex - 1 >= 0)
        {
            CurrentPage = singleton.GamePageList[--gamePageListIndex];
        }
    }
    private async Task FetchPostsAndShowFirstItem()
    {
        try
        {
            isBackgroundWorkerRunning = true;
            WeakReferenceMessenger.Default.Send(new IsLoadingSourceMessage(true));
            await foreach (var gamePage in siteManager.GetFeedFromGalleryPage(CancellationTokenSource.Token))
            {
                gamePage.Name = gamePage.Name.ToCapital();
                singleton.GamePageList.Add(gamePage);
                if (_isLoading)
                {
                    CurrentPage = singleton.GamePageList.ElementAt(++gamePageListIndex);
                    _isLoading = false;
                    WeakReferenceMessenger.Default.Send(new IsLoadingSourceMessage(false));
                }
            }
            isBackgroundWorkerRunning = false;
            WeakReferenceMessenger.Default.Send(new IsLoadingSourceMessage(false));
        }
        catch
        {
            WeakReferenceMessenger.Default.Send(new IsLoadingSourceMessage(false));
            isBackgroundWorkerRunning = false;
        }
    }
}