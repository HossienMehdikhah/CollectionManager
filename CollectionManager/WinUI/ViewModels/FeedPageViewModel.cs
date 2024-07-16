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
using Microsoft.UI.Xaml;
namespace CollectionManager.WinUI.ViewModels;

public partial class FeedPageViewModel(SiteManager siteManager,
    IOptions<CollectionManagerOption> option, 
    FeedPageViewModelSingleton singleton) : ObservableObject, INavigationAware
{
    [ObservableProperty]
    private bool progressRingIsActive = true;
    [ObservableProperty]
    private Visibility progressRingVisibility = Visibility.Visible;
    private int gamePageListIndex = -1;
    private bool isBackgroundWorkerRunning;
    private static GamePageDTO CurrentPage
    {
        set
        {
            WeakReferenceMessenger.Default.Send(new CurrentPageSourceMessage(value));
        }
    }
    private readonly CancellationTokenSource CancellationTokenSource = new();

    public async void OnNavigatedTo(object parameter)
    {
        if (singleton.GamePageList.Count == 0)
            await FetchPostsAndShowFirstItem();
        else
        {
            CurrentPage = singleton.GamePageList[++gamePageListIndex];
            DeactivateLoading();
        }
    }
    public async void OnNavigatedFrom()
    {
        await CancellationTokenSource.CancelAsync();
        singleton.FetchedPostsCount = singleton.FetchedPostsCount;
    }

    [RelayCommand]
    private async Task NextButtonEvent()
    {
        if (gamePageListIndex + 1 <= singleton.GamePageList.Count)
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
        else
        {
            if (isBackgroundWorkerRunning)
                ActivateLoading();
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
            ActivateLoading();
            isBackgroundWorkerRunning = true;
            await foreach (var gamePage in siteManager.GetFeedFromGalleryPage(CancellationTokenSource.Token))
            {
                gamePage.Name = gamePage.Name.ToCapital();
                singleton.GamePageList.Add(gamePage);
                if (ProgressRingIsActive)
                {
                    CurrentPage = singleton.GamePageList.ElementAt(++gamePageListIndex);
                    DeactivateLoading();
                }
            }
            isBackgroundWorkerRunning = false;
            DeactivateLoading();
        }
        catch
        {
            DeactivateLoading();
            isBackgroundWorkerRunning = false;
        }
    }
    private void ActivateLoading()
    {
        ProgressRingIsActive = true;
        ProgressRingVisibility = Visibility.Visible;
    }
    private void DeactivateLoading()
    {
        ProgressRingIsActive = false;
        ProgressRingVisibility = Visibility.Collapsed;
    }
}