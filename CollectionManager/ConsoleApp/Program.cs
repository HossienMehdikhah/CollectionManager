using CollectionManager.Core.Services;

namespace ConsoleApp;

public class Program
{
    public static void Main(string[] args)
    {
        test().Wait();
    }

    private static async Task test()
    {
        var uri = new Uri("https://par30games.net/227066/download-lies-of-p-for-pc/");
        Par30gamesSiteCrawler par = new();
        var temp = await par.GetPageAsync(uri);
    }
}
