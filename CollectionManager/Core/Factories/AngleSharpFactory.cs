using AngleSharp;

namespace CollectionManager.Core.Factories;

public class AngleSharpFactory
{
    public IBrowsingContext CreateDefualt()
    {
        var config = Configuration.Default.WithDefaultLoader();
        BrowsingContext context = new(config);
        return context;
    }
}
