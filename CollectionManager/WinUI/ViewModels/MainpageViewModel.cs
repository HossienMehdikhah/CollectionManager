using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
namespace CollectionManager.WinUI.ViewModels;

public class MainpageViewModel(SiteManager siteManager) : ObservableObject
{
    #region Property_Field
    private GamePageDTO currentPage = new();
    private bool isLoading = false;
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
 
    public bool IsLoading
    {
        get => isLoading;
        set => SetProperty(ref isLoading, value);
    }

    public async Task Init()
    {
        await foreach (var gamePage in siteManager.GetFeedFromGalleryPage())
        {
            gamePageList.Add(gamePage);
            if (gamePageList.Count == 1)
            {
                IsLoading = false;
                gamePageListIndex++;
                CurrentPage = gamePageList.ElementAt(gamePageListIndex);
            }
        }
    }
}
