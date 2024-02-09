namespace CollectionManager.Core.Models;

public class GameSet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public MarkedType MarkedType { get; set; }
    public byte[] Thumbnail { get; set; }
}
