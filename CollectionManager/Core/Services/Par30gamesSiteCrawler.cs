using AngleSharp;
using AngleSharp.Dom;
using CollectionManager.Core.Contracts.Services;
using CollectionManager.Core.Factories;
using CollectionManager.Core.Models;
using Flurl.Http;
using Humanizer;
using System.Globalization;

namespace CollectionManager.Core.Services;
public class Par30gamesSiteCrawler(AngleSharpFactory _angleSharpFactory) : IGameSiteCrawler
{
    private const string baseUrl = "https://par30games.net/";
    private const string feedUrl = baseUrl + "pc/page/{0}/";
    private const string seachUrl = baseUrl + "/page/1/?s=دانلود+{0}+pc";
    private const byte maxPostPerPage = 8;

    public async IAsyncEnumerable<GamePageDTO> GetFeedAsync(uint skipPostCount, uint takePostCount)
    {
        IBrowsingContext context = _angleSharpFactory.CreateDefualt();
        var skipPage = skipPostCount / maxPostPerPage;
        var uri = string.Format(feedUrl, ++skipPage);
        var stringDocument = await uri.GetStringAsync();
        var document = await context.OpenAsync(x => x.Content(stringDocument));
        var Articles = GetArticles(document);
        await foreach (var item in GetGamePagesLinkAsync(Articles.ToArray()))
        {
            yield return item;
        }
    }
    public async Task<GamePageDTO> GetPageAsync(Uri uri)
    {
        GamePageDTO selectedPage = new();
        CancellationToken cancellationToken = CancellationToken.None;
        await foreach (var item in GetGamePagesLinkAsync(uri).WithCancellation(cancellationToken))
        {
            selectedPage = item;
            cancellationToken = new CancellationToken(true);
        }
        return selectedPage;
    }
    public async Task<IEnumerable<GamePageDTO>> GetSearchSuggestionAsync(string query)
    {
        IBrowsingContext context = _angleSharpFactory.CreateDefualt();
        var uri = string.Format(seachUrl, query);
        var htmlDocument = await uri.GetStringAsync();
        var document = await context.OpenAsync(x => x.Content(htmlDocument));
        var Articles = GetArticles(document).Select(uir=>
        {
            return new GamePageDTO
            {
                Name = GetNormalizeName(uir),
                URL = uir,
            };
        });
        return Articles;
    }


    private IEnumerable<Uri> GetArticles(IDocument document)
    {
        var Articles = document.QuerySelectorAll(".post.icon-steam")?.Select(x => GetUri(x));
        //if (!Articles.Any()) throw new Exception("HTML has not Any GamePageLink");
        return Articles;
    }
    private async IAsyncEnumerable<GamePageDTO> GetGamePagesLinkAsync(params Uri[] uriArticles)
    {
        IBrowsingContext context = _angleSharpFactory.CreateDefualt();
        foreach (var uri in uriArticles)
        {
            var pageContentHtml = await uri.GetStringAsync();
            var document = await context.OpenAsync(x => x.Content(pageContentHtml));

            GamePageDTO gamePage = new()
            {
                Name = GetNormalizeName(uri),
                URL = uri,
                PublishDate = GetDateTime(document),
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
            .Select(x => x.TextContent)
            .Where(x => !string.IsNullOrEmpty(x));
        temp = temp.Take(temp.Count() - 4);
        return string.Join("\\n", temp);
    }
    private Uri GetUri(IElement node)
    {
        var hrefElement = node.QuerySelector("header h2 a");
        return new Uri(System.Web.HttpUtility.UrlDecode(hrefElement.Attributes["href"].Value));
    }
    private DateOnly GetDateTime(IDocument node)
    {
        var clockNode = node.QuerySelector(".icon-days").NextSibling;
        var stringDatetime = clockNode.TextContent.Split(' ').Select(x => x.Trim()).ToArray();
        stringDatetime[1] = ConstContainer.PersianMonths.First(x => x.Key.Equals(stringDatetime[1])).Value.ToString();
        var intDatetime = stringDatetime.Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray();
        return new DateOnly(intDatetime[2], intDatetime[1], intDatetime[0], new PersianCalendar());
    }
    private string GetNormalizeName(Uri url)
    {
        var urlSprated = url.ToString().Split('/');
        var temp = urlSprated.ElementAt(urlSprated.Length - 2);
        temp = temp.Replace("download-", "");
        temp = temp.Replace("-for-pc", "");
        temp = temp.Replace("-", " ");
        return temp.Humanize(LetterCasing.Title);
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
