using AngleSharp;
using AngleSharp.Dom;
using CollectionManager.Core.Contracts.Services;
using CollectionManager.Core.Factories;
using CollectionManager.Core.Models;
using Flurl.Http;

namespace CollectionManager.Core.Services;
public class Par30gamesSiteCrawler(AngleSharpFactory _angleSharpFactory) : IGameSiteCrawler
{
    private const string baseUrl = "https://par30games.net/pc/page/{0}/";
    private const byte maxPostPerPage = 8;

    public async IAsyncEnumerable<GamePageDTO> GetFeedAsync(uint skipPostCount, uint takePostCount)
    {
        IBrowsingContext context = _angleSharpFactory.CreateDefualt();
        var skipPage = skipPostCount / maxPostPerPage;
        var uri = string.Format(baseUrl, ++skipPage);
        var htmlDocument = await uri.GetStringAsync();
        var document = await context.OpenAsync(x => x.Content(htmlDocument));
        await foreach (var item in GetGamePagesLinkAsync(document))
        {
            yield return item;
        }
    }


    public async IAsyncEnumerable<GamePageDTO> GetGamePagesLinkAsync(IDocument htmlDocument)
    {
        var Articles = htmlDocument.QuerySelectorAll(".post.icon-steam");
        if (Articles.Length == 0) throw new Exception("HTML has not Any GamePageLink");
        IBrowsingContext context = _angleSharpFactory.CreateDefualt();

        foreach (var Article in Articles)
        {
            var url = GetUrl(Article);
            var pageContentHtml = await url.GetStringAsync();
            var document = await context.OpenAsync(x => x.Content(pageContentHtml));

            GamePageDTO gamePage = new()
            {
                Name = GameNameNormalize(url.ToString()),
                URL = url,
                PublishDate = GetDateTime(Article),
                Content = GetGameContentAsync(document),
            };
            yield return gamePage;
        }
    }

    private GamePageContentDTO GetGameContentAsync(IDocument htmlDocument)
    {
        return new GamePageContentDTO()
        {
            CoverLink = GetGameCover(htmlDocument),
            Summery = GetSummery(htmlDocument),
            GalleryLink = GetGalleryLink(htmlDocument),
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
