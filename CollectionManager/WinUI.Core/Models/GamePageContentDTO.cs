namespace CollectionManager.Core.Models;

public class GamePageContentDTO
{
    private List<Uri> galleryLink = [];

    public Uri? CoverLink { get; set; }
    public string? Summery { get; set; }
    public Uri? VideoLink { get; set; }
    public IEnumerable<Uri> GalleryLink
    {
        get
        {
            List<Uri> galleryItemtemp = new();
            if (VideoLink is not null)
                galleryItemtemp.Add(new("ms-appx:///Assets/PlayIcon200x200.png"));
            if (CoverLink is not null)
                galleryItemtemp.Add(CoverLink);
            if (galleryLink is not null)
                galleryItemtemp.AddRange(galleryLink);
            return galleryItemtemp;
        }
        set => galleryLink = value.ToList();
    }
    public IDictionary<string, IEnumerable<Uri>>? DownloadLink { get; set; }
}
