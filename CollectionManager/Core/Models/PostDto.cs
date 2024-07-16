namespace CollectionManager.Core.Models;

public class PostDTO
{
    public string Name { get; set; } = string.Empty;
    public required Uri URL { get; set; }
    public Uri Thumbnail { get; set; }
}
