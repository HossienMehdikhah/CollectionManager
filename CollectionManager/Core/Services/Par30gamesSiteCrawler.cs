using AngleSharp;
using AngleSharp.Dom;
using CollectionManager.Core.Contracts.Services;
using CollectionManager.Core.Factories;
using CollectionManager.Core.Models;
using CollectionManager.Core.Utilities;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using System;
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<PostDTO> GetPostsAsync(uint skip, uint take,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var skipPage = skip / maxPostPerPage;
        Uri uri = new(string.Format(feedUrl, ++skipPage));
        foreach (var item in await GetPostsAsyncLocal(uri))
        {
            yield return item;
        } 

        Task<IEnumerable<PostDTO>> GetPostsAsyncLocal(Uri uri)
        {
            try
            {
                return GetPostsAsync(uri, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                return Task.FromResult(new List<PostDTO>().AsEnumerable());
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="posts"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<GamePageDTO> GetGamePagesAsync(IEnumerable<PostDTO> posts,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        IBrowsingContext context = AngleSharpFactory.CreateDefault();
        foreach (var post in posts)
        {
            var gamePage = await GetGamePagesAsyncLocal(post);
            if (gamePage is not null)
                yield return gamePage;
        }

        async Task<GamePageDTO?> GetGamePagesAsyncLocal(PostDTO post)
        {
            try
            {
                return await GetGamePageAsync(post, context, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                return await Task.FromResult(null as GamePageDTO);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public Task<GamePageDTO> GetPageAsync(Uri uri)
    {
        PostDTO post = new()
        {
            URL = uri,
            Name = ExtractionNameFromURL(uri),
        };
        return GetGamePageAsync(post);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<PostDTO> SearchAsync(string query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var uri = string.Format(seachUrl, query);
        foreach (var post in await SearchAsyncLocal())
        {
            yield return post;
        }

        Task<IEnumerable<PostDTO>> SearchAsyncLocal()
        {
            try
            {
                return GetPostsAsync(new Uri(uri), cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                return Task.FromResult(new List<PostDTO>().AsEnumerable());
            }
        }
    }

    #region Posts
    /// <summary>
    /// 
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<IEnumerable<PostDTO>> GetPostsAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        var browsingContext = AngleSharpFactory.CreateDefault();
        var stringDocument = await uri.GetStringAsync(cancellationToken: cancellationToken);
        var document = await browsingContext.OpenAsync(x => x.Content(stringDocument), cancellationToken);
        var Articles = ExtractionPosts(document);
        return Articles;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
    /// <exception cref="HtmlSectionNotFoundException"></exception>
    private List<PostDTO> ExtractionPosts(IDocument document)
    {
        var Articles = document.QuerySelectorAll(".post.icon-steam");
        if (Articles.Length == 0)
            throw new HtmlSectionNotFoundException();
        List<PostDTO> posts = [];
        foreach (var Article in Articles)
        {
            try
            {
                var url = ExtractionPostUrl(Article);
                posts.Add(new PostDTO
                {
                    Name = ExtractionNameFromURL(url),
                    URL = url,
                    Thumbnail = ExtractionPostThumbnail(Article),
                });
            }
            catch (HtmlSectionNotFoundException ex)
            {
                logger.LogError(ex, "");
                continue;
            }
        }
        return posts;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    /// <exception cref="HtmlSectionNotFoundException"></exception>
    private static Uri ExtractionPostUrl(IElement node)
    {
        var hrefElement = node.QuerySelector("header h2 a") ?? throw new HtmlSectionNotFoundException();
        var link = hrefElement.Attributes["href"] ?? throw new HtmlSectionNotFoundException();
        return new Uri(System.Web.HttpUtility.UrlDecode(link.Value));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    /// <exception cref="HtmlSectionNotFoundException"></exception>
    private string ExtractionNameFromURL(Uri url)
    {
        try
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
        catch (IndexOutOfRangeException ex)
        {
            logger.LogError(ex, "");
            throw new HtmlSectionNotFoundException();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    /// <exception cref="HtmlSectionNotFoundException"></exception>
    private static Uri ExtractionPostThumbnail(IElement node)
    {
        var imgElement = node.QuerySelector("a img") ?? throw new HtmlSectionNotFoundException();
        var link = imgElement.Attributes["data-src"]
            ?? imgElement.Attributes["src"]
            ?? throw new HtmlSectionNotFoundException();
        return new Uri(System.Web.HttpUtility.UrlDecode(link.Value));
    }
    [GeneratedRegex("(?:download-)?" +
       "(?:(?<gameName>[A-Za-z0-9-]*)-for-pc" +
       "|(?<gameName>[A-Za-z0-9-]*)" +
       "|(?<gameName>[A-Za-z0-9-]*)-pc)")]
    private static partial Regex GetGameNameFromURLPattern();
    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="post"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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
            Thumbnail = ParseThumbnailGameLink(document),
            PublishDate = GetDateTime(document),
            CoverLink = GetGameCover(document),
            Summery = GetSummery(document),
            GalleryLink = ParseGalleryLink(document),
            DownloadLink = ParseEncoder(document),
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    /// <exception cref="HtmlSectionNotFoundException"></exception>
    private DateOnly GetDateTime(IDocument node)
    {
        var clockNode = node.QuerySelector(".icon-days")?.NextSibling ?? throw new HtmlSectionNotFoundException();
        var stringDatetime = clockNode.TextContent.Split(' ').Select(x => x.Trim()).ToArray();
        try
        {
            stringDatetime[1] = ConstContainer.PersianMonths.First(x => x.Key.Equals(stringDatetime[1])).Value.ToString();
            var intDatetime = stringDatetime.Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray();
            return new DateOnly(intDatetime[2], intDatetime[1], intDatetime[0], new PersianCalendar());
        }
        catch (IndexOutOfRangeException ex)
        {
            logger.LogError(ex, "");
            throw new HtmlSectionNotFoundException();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
    /// <exception cref="HtmlSectionNotFoundException"></exception>
    private static Uri GetGameCover(IDocument document)
    {
        var imageUrl = document
            .QuerySelectorAll(".thumby.wp-post-image")
            .Where(x => x.Attributes["src"] is not null)
            .Select(x => x.Attributes["src"]!.Value)
            .First(x => x.StartsWith("https"));
        return imageUrl is not null ? new Uri(imageUrl) : throw new HtmlSectionNotFoundException();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
    private static IEnumerable<Uri> ParseGalleryLink(IDocument document)
    {
        var imagesUrl = document
            .QuerySelectorAll("div.gallery-icon.landscape > a")
            .Select(x => x.Attributes["href"]?.Value);
        return imagesUrl.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Uri(x!));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="htmlDocument"></param>
    /// <returns></returns>
    private List<EncoderTeamDto> ParseEncoder(IDocument htmlDocument)
    {
        var temp = htmlDocument.QuerySelectorAll(".buttondl");
        var temp1 = htmlDocument.QuerySelectorAll(".buttondl-tab .tab-content div > ul");

        List<EncoderTeamDto> encoderTeams = [];
        for (int i = 0; i < temp.Length; i++)
        {
            try
            {
                var item = temp[i];
                EncoderTeamDto newEncoder = new()
                {
                    EncoderName = ParseEncoderTeamName(item),
                    TotalValue = ParsTotalValue(item),
                    EncoderPackages = ParseEncoderAndDownloadLinks(temp1[i]),
                };
                encoderTeams.Add(newEncoder);
            }
            catch (HtmlSectionNotFoundException html)
            {
                logger.LogError(html, "");
            }
        }
        return encoderTeams;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="div"></param>
    /// <returns></returns>
    /// <exception cref="HtmlSectionNotFoundException"></exception>
    private static IEnumerable<EncoderPackageDTO> ParseEncoderAndDownloadLinks(IElement div)
    {
        Dictionary<string, List<Uri>> dic = [];
        List<Uri> links = [];
        string lastNode = string.Empty;
        foreach (var item in div.Children)
        {
            //Parse EncoderName
            if (item.NodeName.Equals("B"))
            {
                if (lastNode != string.Empty)
                {
                    dic.Add(lastNode, links);
                    links = [];
                }
                lastNode = RegexHelper.TakeBetweenQuote(item.OuterHtml);
            }
            //Parse Links
            else
            {
                var element = item.Children[0] ?? throw new HtmlSectionNotFoundException();
                var link = element.Attributes["href"]?.Value ?? throw new HtmlSectionNotFoundException();
                links.Add(new Uri(link));
            }
        }
        dic.Add(lastNode, links);
        uint counter = 1;
        return dic.Select(x => new EncoderPackageDTO
        {
            EncoderPackageName = x.Key,
            DownloadLink = x.Value.Select(y => new DownloadURIDTO
            {
                PartNumber = $"Part {counter++}",
                Uri = y
            }),
        });
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
    private static string ParseEncoderTeamName(IElement document)
    {
        var text = RegexHelper.RemoveWhiteSpace(document.TextContent);
        var encoderName = string.IsNullOrWhiteSpace(document.Children[1].InnerHtml)
            ? text : text.Replace(document.Children[1].InnerHtml, "").Trim();
        return encoderName;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
    /// <exception cref="HtmlSectionNotFoundException"></exception>
    private static string ParsTotalValue(IElement document)
    {
        var value = RegexHelper.TakeNumber(document.TextContent);
        if (string.IsNullOrWhiteSpace(value)) throw new HtmlSectionNotFoundException();
        if (document.TextContent.Contains("گیگابایت"))
            value += " GB";
        else if (document.TextContent.Contains("مگابایت"))
            value += " MB";
        else
            value += "Unkown";
        return value;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
    /// <exception cref="HtmlSectionNotFoundException"></exception>
    private static Uri ParseThumbnailGameLink(IDocument document)
    {
        var postHeaderNode = document.QuerySelector(".review > div.pic > img") ?? throw new HtmlSectionNotFoundException();
        var imageLink = postHeaderNode.Attributes["data-src"] ?? throw new HtmlSectionNotFoundException();
        var link = System.Web.HttpUtility.UrlDecode(imageLink.Value);
        return new Uri(link!);
    }
}