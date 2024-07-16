using CollectionManager.Core.Models;
namespace CollectionManager.WinUI.Singleton;

public class SearchPageViewModelSingleton
{
    public string QuerySearch { get; set; } = string.Empty;
    public List<GamePageDTO> GamePages { get; set; } = [];
}