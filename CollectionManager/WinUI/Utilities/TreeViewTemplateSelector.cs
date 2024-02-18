using CollectionManager.Core.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CollectionManager.Core.Utilities;

public class TreeViewTemplateSelector : DataTemplateSelector
{
    public DataTemplate? EncoderTeamNameTemplate { get; set; }
    public DataTemplate? PackageNameTemplate { get; set; }
    public DataTemplate? DownloadLinkTemplate { get; set; }

    protected override DataTemplate? SelectTemplateCore(object item)
    {
        return item switch
        {
            EncoderTeamDto => EncoderTeamNameTemplate,
            EncoderPackageDTO => PackageNameTemplate,
            DownloadURIDTO => DownloadLinkTemplate,
            _ => throw new NotSupportedException(),
        };
    }
}
