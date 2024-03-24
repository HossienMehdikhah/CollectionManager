namespace CollectionManager.Core.Models;

public class GamePageDTO
{
    private List<Uri> galleryLink = [];

    public string Name { get; set; } = "No Name";
    public Uri? URL { get; set; }
    public Uri? Thumbnail { get; set; }
    public DateOnly PublishDate { get; set; }
    public MarkedType MarkedType { get; set; }
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
    public IEnumerable<EncoderTeamDto> DownloadLink { get; set; } = [];
}
