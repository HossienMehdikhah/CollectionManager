namespace CollectionManager.Core.Models;

public class EncoderTeamDto
{
    public string EncoderName { get; set; } = string.Empty;
    public IEnumerable<EncoderPackageDTO> EncoderPackages { get; set; } = [];
    public string TotalValue { get; set; } = string.Empty;
}
