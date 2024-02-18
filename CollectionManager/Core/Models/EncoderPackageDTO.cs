namespace CollectionManager.Core.Models;

public class EncoderPackageDTO
{
    public string EncoderPackageName { get; set; } = string.Empty;
    public IEnumerable<DownloadURIDTO> DownloadLink { get; set; } = [];
}
