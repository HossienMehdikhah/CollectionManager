using System.ComponentModel.DataAnnotations;
namespace CollectionManagerWebService.Endpoints;

public class DownloadGameLinksDTO : IValidatableObject
{
    public required string GameName { get; set; }
    [Range(1, 10)]
    public byte Praiority { get; set; } = 1;
    public List<string> Links { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var link in Links)
            if(!Uri.TryCreate(link, new UriCreationOptions() { DangerousDisablePathAndQueryCanonicalization = true }, out _))
                yield return new ValidationResult($"Link Format Is Incorrect: {link}");
    }
}
