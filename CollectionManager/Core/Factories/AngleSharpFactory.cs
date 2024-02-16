using AngleSharp;

namespace CollectionManager.Core.Factories;

public class AngleSharpFactory
{
    public static IBrowsingContext CreateDefault()
    {
        var config = Configuration.Default.WithDefaultLoader();
        BrowsingContext context = new(config);
        return context;
    }
}
