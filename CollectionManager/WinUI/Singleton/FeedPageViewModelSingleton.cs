using CollectionManager.Core.Models;

namespace CollectionManager.WinUI.Singleton;
public class FeedPageViewModelSingleton
{
    private FeedPageViewModelSingleton()
    {
        
    }
    public static FeedPageViewModelSingleton Build()
    {
        return new FeedPageViewModelSingleton();
    }
    public List<GamePageDTO> GamePageList { get; init; } = [];
}
