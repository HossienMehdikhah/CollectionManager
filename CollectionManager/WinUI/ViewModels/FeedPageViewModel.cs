using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CollectionManager.Core.Utilities;
using CollectionManager.WinUI.Singleton;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;
namespace CollectionManager.WinUI.ViewModels;

public partial class FeedPageViewModel(SiteManager siteManager,
    IOptions<CollectionManagerOption> option, 
    FeedPageViewModelSingleton singleton) : ObservableObject
{
    private GamePageDTO currentPage = new();
    [ObservableProperty]
    private bool progressRingIsActive = true;
    [ObservableProperty]
    private Visibility progressRingVisibility = Visibility.Visible;
    private int gamePageListIndex = -1;
    private bool isBackgroundWorkerRunning;

    public GamePageDTO CurrentPage
    {
        get => gamePageListIndex < 0 ? currentPage : singleton.GamePageList[gamePageListIndex];
        set
        {
            SetProperty(ref currentPage, value);
        }
    }
    public CancellationToken CancellationToken { get; set; }

    [RelayCommand]
    public async Task Loading()
    {
        if(singleton.GamePageList.Count == 0)
            await FetchPostsAndShowFirstItem();
        else
        {
            CurrentPage = singleton.GamePageList[++gamePageListIndex];
            DeactivateLoading();
        }
    }
    [RelayCommand]
    public void Unloaded()
    {
        CancellationToken = new CancellationToken(true);
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
                await foreach (var gamePage in siteManager.GetFeedFromGalleryPage(CancellationToken))
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
            await foreach (var gamePage in siteManager.GetFeedFromGalleryPage(CancellationToken))
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
