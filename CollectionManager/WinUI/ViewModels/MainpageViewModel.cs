using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;
namespace CollectionManager.WinUI.ViewModels;

public partial class MainpageViewModel(SiteManager siteManager,
    IOptions<CollectionManagerOption> option) : ObservableObject
{
    private GamePageDTO currentPage = new();
    [ObservableProperty]
    private bool progressRingIsActive = true;
    [ObservableProperty]
    private Visibility progressRingVisibility = Visibility.Visible;
    private readonly List<GamePageDTO> gamePageList = [];
    private readonly CollectionManagerOption option = option.Value;
    private int gamePageListIndex = -1;
    private bool isBackgroundWorkerRunning;

    public GamePageDTO CurrentPage
    {
        get => gamePageListIndex < 0 ? currentPage : gamePageList[gamePageListIndex];
        set
        {
            SetProperty(ref currentPage, value);
        }
    }
    public CancellationToken CancellationToken { get; set; }

    [RelayCommand]
    public async Task Loading()
    {
        await FetchPostsAndShowFirstItem();
    }    
    [RelayCommand]
    private async Task NextButtonEvent()
    {
        if (gamePageListIndex + 2 <= gamePageList.Count)
        {
            CurrentPage = gamePageList[++gamePageListIndex];
            if (!isBackgroundWorkerRunning 
                && gamePageList.Count - (gamePageListIndex + 1) <= option.SearchThreshold)
            {
                isBackgroundWorkerRunning = true;
                await foreach (var gamePage in siteManager.GetFeedFromGalleryPage(CancellationToken))
                {
                    gamePageList.Add(gamePage);
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
        if (gamePageListIndex >= 0)
        {
            CurrentPage = gamePageList[--gamePageListIndex];
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
                gamePageList.Add(gamePage);
                if (ProgressRingIsActive)
                {
                    gamePageListIndex++;
                    CurrentPage = gamePageList.ElementAt(gamePageListIndex);
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
