using CollectionManager.Core.Models;
namespace CollectionManager.WinUI.Singleton;

public class FeedPageViewModelSingleton
{
    public List<GamePageDTO> GamePageList { get; init; } = [];
    public uint FetchedPostsCount { get; set; }
}
