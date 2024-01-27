using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Newtonsoft.Json.Linq;
namespace CollectionManager.WinUI.ViewModels;

public class MainpageViewModel(SiteManager siteManager) : ObservableObject
{
    #region Property_Field
    private GamePageDTO currentPage = new();
    private bool progressRingIsActive = true;
    private Visibility progressRingVisibility = Visibility.Visible;
    private bool nextButtonIsEnable = false;
    #endregion

    private readonly List<GamePageDTO> gamePageList = [];
    private int gamePageListIndex = -1;

    public GamePageDTO CurrentPage
    {
        get => gamePageListIndex < 0 ? currentPage : gamePageList.ElementAt(gamePageListIndex);
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
    public bool NextButtonIsEnable
    {
        get => nextButtonIsEnable;
        private set
        {
            SetProperty(ref nextButtonIsEnable, value);
        }
    }

    public async Task Init()
    {
        await foreach (var gamePage in siteManager.GetFeedFromGalleryPage())
        {
            gamePageList.Add(gamePage);
            if (gamePageList.Count == 1)
            {
                gamePageListIndex++;
                CurrentPage = gamePageList.ElementAt(gamePageListIndex);
                Deactivate();
            }
        }
    }

    private void Activate()
    {
        ProgressRingIsActive = true;
        ProgressRingVisibility = Visibility.Visible;
        NextButtonIsEnable = false;
    }
    private void Deactivate()
    {
        ProgressRingIsActive = false;
        ProgressRingVisibility = Visibility.Collapsed;
        NextButtonIsEnable = true;
    }
}
