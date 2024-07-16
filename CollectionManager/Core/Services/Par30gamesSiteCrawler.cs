using AngleSharp;
using AngleSharp.Dom;
using CollectionManager.Core.Contracts.Services;
using CollectionManager.Core.Factories;
using CollectionManager.Core.Models;
using CollectionManager.Core.Utilities;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
namespace CollectionManager.Core.Services;

public partial class Par30gamesSiteCrawler(ILogger<Par30gamesSiteCrawler> logger) : IGameSiteCrawler
{
    private const string baseUrl = "https://par30games.net/";
    private const string feedUrl = baseUrl + "pc/page/{0}/";
    private const string seachUrl = baseUrl + "/?s=+دانلود+{0}+pc";
    private const byte maxPostPerPage = 8;

    public async Task<IEnumerable<PostDTO>> GetPostsAsync(uint skip, uint take,
        CancellationToken cancellationToken)
    {
        try
        {
            var skipPage = skip / maxPostPerPage;
            Uri uri = new(string.Format(feedUrl, ++skipPage));
            var posts = await GetPostsAsync(uri, cancellationToken);
            return posts;
        }
        catch
        {
            throw;
        }
    }
    public async IAsyncEnumerable<GamePageDTO> GetGamePagesAsync(IEnumerable<PostDTO> posts,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        IBrowsingContext context = AngleSharpFactory.CreateDefault();
        foreach (var post in posts)
        {
            GamePageDTO gamepage = new();
            try
            {
                gamepage = await GetGamePageAsync(post, context, cancellationToken);
            }
            catch (NotFundHtmlSectionException e)
            {
                logger.LogError(e, "");
                continue;
            }
            catch
            {
                throw;
            }
            yield return gamepage;
        }
    }
    public Task<GamePageDTO> GetPageAsync(Uri uri)
    {
        PostDTO post = new()
        {
            URL = uri,
            Name = ExtractionNameFromURL(uri),
        };
        return GetGamePageAsync(post);
    }
    public async IAsyncEnumerable<PostDTO> SearchAsync(string query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var uri = string.Format(seachUrl, query);
        foreach (var post in await GetPostsAsync(new Uri(uri), cancellationToken))
        {
            yield return post;
        }
    }

    #region Posts
    private async Task<IEnumerable<PostDTO>> GetPostsAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        try
        {
            var browsingContext = AngleSharpFactory.CreateDefault();
            var stringDocument = await uri.GetStringAsync(cancellationToken: cancellationToken);
            var document = await browsingContext.OpenAsync(x => x.Content(stringDocument), cancellationToken);
            var Articles = ExtractionPosts(document);
            return Articles;
        }
        catch (NotFundHtmlSectionException e)
        {
            logger.LogWarning(e, "");
            return [];
        }
        catch
        {
            throw;
        }
    }
    private static List<PostDTO> ExtractionPosts(IDocument document)
    {
        var Articles = document.QuerySelectorAll(".post.icon-steam");
        if (Articles.Length == 0)
            throw new NotFundHtmlSectionException();
        List<PostDTO> posts = [];
        foreach (var Article in Articles)
        {
            var url = ExtractionPostUrl(Article);
            posts.Add(new PostDTO
            {
                Name = ExtractionNameFromURL(url),
                URL = url,
                Thumbnail = ExtractionPostThumbnail(Article),
            });
        }
        return posts;
    }
    private static Uri ExtractionPostUrl(IElement node)
    {
        var hrefElement = node.QuerySelector("header h2 a") ?? throw new NotFundHtmlSectionException();
        var link = hrefElement.Attributes["href"] ?? throw new NotFundHtmlSectionException();
        return new Uri(System.Web.HttpUtility.UrlDecode(link.Value));
    }
    private static string ExtractionNameFromURL(Uri url)
    {
        var rawTitle = url.Segments[2].Replace("/", "");
        var result = GetGameNameFromURLPattern()
                .Matches(rawTitle)
                .SelectMany(x => x.Groups.Values)
                .Where(x => !string.IsNullOrEmpty(x.Value))
                .First(x => x.Name == "gameName").Value;

        result = result.Replace("-", " ");
        var gameName = RegexHelper.ConvertRomanNumberToEnglish_Under40(result);
        return gameName;
    }
    [GeneratedRegex("(?:download-)?" +
        "(?:(?<gameName>[A-Za-z0-9-]*)-for-pc" +
        "|(?<gameName>[A-Za-z0-9-]*)" +
        "|(?<gameName>[A-Za-z0-9-]*)-pc)")]
    private static partial Regex GetGameNameFromURLPattern();
    private static Uri ExtractionPostThumbnail(IElement node)
    {
        var imgElement = node.QuerySelector("a img") ?? throw new NotFundHtmlSectionException();
        var link = imgElement.Attributes["data-src"]
            ?? imgElement.Attributes["src"]
            ?? throw new NotFundHtmlSectionException();
        return new Uri(System.Web.HttpUtility.UrlDecode(link.Value));
    }
    #endregion

    private async Task<GamePageDTO> GetGamePageAsync(PostDTO post, IBrowsingContext? context = null,
        CancellationToken cancellationToken = default)
    {
        context ??= AngleSharpFactory.CreateDefault();
        var pageContentHtml = await post.URL.GetStringAsync(cancellationToken: cancellationToken);
        var document = await context.OpenAsync(x => x.Content(pageContentHtml), cancellationToken);
        var temp = new GamePageDTO()
        {
            URL = post.URL,
            Name = post.Name,
            Thumbnail = GetThumbnail(document),
            PublishDate = GetDateTime(document),
            CoverLink = GetGameCover(document),
            Summery = GetSummery(document),
            GalleryLink = GetGalleryLink(document),
            DownloadLink = GetDownloadLink(document),
        };
        return temp;
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
    private DateOnly GetDateTime(IDocument node)
    {
        var clockNode = node.QuerySelector(".icon-days").NextSibling;
        var stringDatetime = clockNode.TextContent.Split(' ').Select(x => x.Trim()).ToArray();
        stringDatetime[1] = ConstContainer.PersianMonths.First(x => x.Key.Equals(stringDatetime[1])).Value.ToString();
        var intDatetime = stringDatetime.Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray();
        return new DateOnly(intDatetime[2], intDatetime[1], intDatetime[0], new PersianCalendar());
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
    private IEnumerable<EncoderTeamDto> GetDownloadLink(IDocument htmlDocument)
    {
        var temp = htmlDocument.QuerySelectorAll(".buttondl");
        var temp1 = htmlDocument.QuerySelectorAll(".buttondl-tab .tab-content div > ul");

        List<EncoderTeamDto> encoderTeams = [];
        for (int i = 0; i < temp.Length; i++)
        {
            var item = temp[i];
            EncoderTeamDto newEncoder = new()
            {
                EncoderName = GetEncoderTeamName(item),
                TotalValue = GetTotalValue(item),
                EncoderPackages = GetDownloadAndUpdateLinks(temp1[i]),
            };
            encoderTeams.Add(newEncoder);
        }
        return encoderTeams;
    }
    private IEnumerable<EncoderPackageDTO> GetDownloadAndUpdateLinks(IElement div)
    {
        Dictionary<string, List<Uri>> dic = [];
        List<Uri> links = [];
        string lastNode = string.Empty;
        foreach (var item in div.Children)
        {
            if (item.NodeName.Equals("B"))
            {
                if (lastNode != string.Empty)
                {
                    dic.Add(lastNode, links);
                    links = [];
                }
                lastNode = RegexHelper.TakeBetweenQuote(item.OuterHtml);
            }
            else links.Add(new Uri(item.Children[0].Attributes["href"].Value));
        }
        dic.Add(lastNode, links);
        return dic.Select(x => new EncoderPackageDTO
        {
            EncoderPackageName = x.Key,
            DownloadLink = x.Value.Select(y =>
            {
                uint counter = 1;
                return new DownloadURIDTO { PartNumber = $"Part {counter}", Uri = y };
            }),
        });
    }
    private string GetEncoderTeamName(IElement document)
    {
        var text = RegexHelper.RemoveWhiteSpace(document.TextContent);
        var encoderName = string.IsNullOrWhiteSpace(document.Children[1].InnerHtml) 
            ? text : text.Replace(document.Children[1].InnerHtml, "").Trim();
        return encoderName;
    }
    private string GetTotalValue(IElement document)
    {
        var value = RegexHelper.TakeNumber(document.TextContent);
        if (document.TextContent.Contains("گیگابایت"))
            value += " GB";
        else if (document.TextContent.Contains("مگابایت"))
            value += " MB";
        return value;
    }





    private Uri GetThumbnail(IDocument document)
    {
        var postHeaderNode = document.QuerySelector(".review > div.pic > img") ?? throw new Exception();
        var link = System.Web.HttpUtility.UrlDecode(postHeaderNode.Attributes["data-src"]?.Value);
        return new Uri(link!);
    }
}