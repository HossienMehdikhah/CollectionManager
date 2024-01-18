namespace CollectionManager.Core.Models;

public class GamePageDTO
{
    public string Name { get; set; }= string.Empty;
    public Uri? URL { get; set; }
    public DateOnly PublishDate { get; set; }
    public MarkedType MarkedType { get; set; }
    public GamePageContentDTO Content { get; set; } = new();
    //public Image? Thumbnail { get; set; }
}
