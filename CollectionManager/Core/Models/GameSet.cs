namespace CollectionManager.Core.Models;

public class GameSet
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required Uri Uri { get; set; }
    public Uri? Thumbnail { get; set; }
    public MarkedType MarkedType { get; set; }
}
