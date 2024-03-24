using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;
namespace CollectionManager.WinUI.ViewModels;

public class MainpageViewModel : ObservableObject
{
    #region Property_Field
    private GamePageDTO currentPage = new();
    private bool progressRingIsActive = true;
    private Visibility progressRingVisibility = Visibility.Visible;
    #endregion

    private readonly List<GamePageDTO> gamePageList = [];
    private readonly SiteManager siteManager;
    private readonly CollectionManagerOption option;
    private int gamePageListIndex = -1;
    private bool isBackgroundWorkerRunning;

    public MainpageViewModel(SiteManager siteManager, IOptions<CollectionManagerOption> option)
    {
        this.siteManager = siteManager;
        this.option = option.Value;
        NextButtonCommand = new AsyncRelayCommand(NextButtonEvent);
        PreviousButtonCommand = new RelayCommand(PreviousButtonEvent);
    }

    public GamePageDTO CurrentPage
    {
        get => gamePageListIndex < 0 ? currentPage : gamePageList[gamePageListIndex];
        set
        {
            SetProperty(ref currentPage, value);
        }
    }
    public bool ProgressRingIsActive
    {
        get => progressRingIsActive;
        private set
        {
            SetProperty(ref progressRingIsActive, value);
        }
    }
    public Visibility ProgressRingVisibility
    {
        get => progressRingVisibility;
        private set
        {
            SetProperty(ref progressRingVisibility, value);
        }
    }
    public IAsyncRelayCommand NextButtonCommand { get; set; }
    public IRelayCommand PreviousButtonCommand { get; set; }

    public async Task Init()
    {
        await FetchPostsAndShowFirstItem();
    }
    


    #region Command
    private async Task NextButtonEvent()
    {
        if (gamePageListIndex + 2 <= gamePageList.Count)
        {
            CurrentPage = gamePageList[++gamePageListIndex];
            if (!isBackgroundWorkerRunning && gamePageList.Count - (gamePageListIndex + 1) <= option.SearchThreshold)
            {
                isBackgroundWorkerRunning = true;
                await foreach (var gamePage in siteManager.GetFeedFromGalleryPage())
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
    private void PreviousButtonEvent()
    {
        if (gamePageListIndex >= 0)
        {
            CurrentPage = gamePageList[--gamePageListIndex];
        }
    }
    #endregion


    private async Task FetchPostsAndShowFirstItem()
    {
        ActivateLoading();
        isBackgroundWorkerRunning = true;
        await foreach (var gamePage in siteManager.GetFeedFromGalleryPage())
        {
            gamePageList.Add(gamePage);
            if (progressRingIsActive)
            {
                gamePageListIndex++;
                CurrentPage = gamePageList.ElementAt(gamePageListIndex);
                DeactivateLoading();
            }
        }
        isBackgroundWorkerRunning = false;
        DeactivateLoading();
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
