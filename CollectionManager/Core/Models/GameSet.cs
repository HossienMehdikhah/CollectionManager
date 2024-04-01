namespace CollectionManager.Core.Models;

public class GameSet
{
    public GameSet()
    {
        var now = DateTime.Now;
        AddedDate = new DateOnly(now.Year, now.Month, now.Day);
    }
    public int Id { get; set; }
    public required string Name { get; set; }
    public required Uri Uri { get; set; }
    public required Uri Thumbnail { get; set; }
    public MarkedType MarkedType { get; set; }
    public DateOnly AddedDate { get; set; } 
}
