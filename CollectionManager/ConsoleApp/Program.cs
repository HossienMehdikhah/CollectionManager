using CollectionManager.Core.Managers;
using CollectionManager.Core.Services;
using Flurl.Http;

namespace ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        Par30gamesHtmlParserService parser = new();
        SiteManager siteManager = new(parser);
       // var temp = siteManager.GetFeedFromGalleryPage().Result;
        var temp1 = "https://par30games.net/240029/download-estate-agent-simulator-for-pc/".GetStringAsync().Result;
        var temp2 = parser.GetGamePageAsync(temp1).Result;
        Console.WriteLine("Hello, World!");
    }
}
