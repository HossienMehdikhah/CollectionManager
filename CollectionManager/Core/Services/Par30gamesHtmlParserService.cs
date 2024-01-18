using AngleSharp;
using AngleSharp.Dom;
using CollectionManager.Core.Contracts.Services;
using CollectionManager.Core.Models;

namespace CollectionManager.Core.Services;
public class Par30gamesHtmlParserService : IGameSiteCrawler
{
    private const string baseUrl = "https://par30games.net/";

    public string CollectionPageURL => baseUrl + "pc";
    public async Task<IEnumerable<GamePageDTO>> GetGamePagesLinkAsync(string htmlDocument)
    {
        var config = Configuration.Default.WithDefaultLoader();
        BrowsingContext context = new(config);
        var document = await context.OpenAsync(x => x.Content(htmlDocument));
        var Articles = document.QuerySelectorAll(".post.icon-steam");
        if (Articles.Length == 0) throw new Exception("HTML has not Any GamePageLink");
        List<GamePageDTO> pages = [];
        foreach (var Article in Articles)
        {
            try
            {
                var url = GetUrl(Article);
                GamePageDTO gamePage = new()
                {
                    Name = GameNameNormalize(url.ToString()),
                    URL = url,
                    PublishDate = GetDateTime(Article),
                };
                pages.Add(gamePage);
            }
            catch (Exception ex)
            {

            }
        }
        return pages;
    }
    public async Task<GamePageContentDTO> GetGamePageAsync(string htmlDocument)
    {
        var config = Configuration.Default.WithDefaultLoader();
        BrowsingContext context = new(config);
        var document = await context.OpenAsync(x => x.Content(htmlDocument));
        return new GamePageContentDTO()
        {
            CoverLink = GetGameCover(document),
            Summery = GetSummery(document),
            GalleryLink = GetGalleryLink(document),
        };
    }

    private string GetSummery(IDocument document)
    {
        var temp = document.QuerySelector(".post-content")
            .QuerySelectorAll("p:not([attribute])")
            .Select(x => x.InnerHtml)
            .Where(x => !string.IsNullOrEmpty(x));
        temp = temp.Take(temp.Count() - 4);
        return string.Join("\\n", temp);
    }
    private Uri GetUrl(IElement node)
    {
        var hrefElement = node.QuerySelector("header h2 a");
        return new Uri(System.Web.HttpUtility.UrlDecode(hrefElement.Attributes["href"].Value));
    }
    private DateOnly GetDateTime(IElement node)
    {
        var clockNode = node.QuerySelectorAll("ul li").First(x => x.TextContent.Contains("تاریخ انتشار"));
        var stringDatetime = clockNode.TextContent.Split(':')[1];
        return DateOnly.Parse(stringDatetime);
    }
    private string GameNameNormalize(string url)
    {
        var urlSprated = url.Split('/');
        var temp = urlSprated.ElementAt(urlSprated.Length - 2);
        temp = temp.Replace("download-", "");
        temp = temp.Replace("-for-pc", "");
        temp = temp.Replace("-", " ");
        return temp;
    }
    private Uri? GetGameCover(IDocument document)
    {
        var imageUrl = document
            .QuerySelectorAll(".thumby.wp-post-image")
            .Select(x => x.Attributes["src"].Value)
            .First(x => x.StartsWith("https"));
        return new Uri(imageUrl);
    }
    private IEnumerable<Uri>? GetGalleryLink(IDocument document)
    {
        var imagesUrl = document
            .QuerySelectorAll("div.gallery-icon.landscape > a")
            .Select(x => x.Attributes["href"].Value);
        return imagesUrl.Select(x => new Uri(x));
    }
}
