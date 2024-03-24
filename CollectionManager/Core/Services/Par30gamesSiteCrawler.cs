using AngleSharp;
using AngleSharp.Dom;
using CollectionManager.Core.Contracts.Services;
using CollectionManager.Core.Factories;
using CollectionManager.Core.Models;
using CollectionManager.Core.Utilities;
using Flurl.Http;
using Humanizer;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace CollectionManager.Core.Services;
public partial class Par30gamesSiteCrawler() : IGameSiteCrawler
{
    private const string baseUrl = "https://par30games.net/";
    private const string feedUrl = baseUrl + "pc/page/{0}/";
    private const string seachUrl = baseUrl + "/page/1/?s=دانلود+{0}+pc";
    private const byte maxPostPerPage = 8;

    public async IAsyncEnumerable<GamePageDTO> GetFeedAsync(uint skipPostCount, uint takePostCount)
    {
        IBrowsingContext context = AngleSharpFactory.CreateDefault();
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
        IBrowsingContext context = AngleSharpFactory.CreateDefault();
        var uri = string.Format(seachUrl, query);
        var htmlDocument = await uri.GetStringAsync();
        var document = await context.OpenAsync(x => x.Content(htmlDocument));
        var Articles = GetArticles(document).Select(uir =>
        {
            return new GamePageDTO
            {
                Name = GetNameFromURL(uir),
                URL = uir,
            };
        });
        return Articles;
    }


    private IEnumerable<Uri> GetArticles(IDocument document)
    {
        var Articles = document.QuerySelectorAll(".post.icon-steam")?.Select(x => GetUri(x));
        return Articles;
    }
    private async IAsyncEnumerable<GamePageDTO> GetGamePagesLinkAsync(params Uri[] uriArticles)
    {
        IBrowsingContext context = AngleSharpFactory.CreateDefault();
        foreach (var uri in uriArticles)
        {
            var pageContentHtml = await uri.GetStringAsync();
            var document = await context.OpenAsync(x => x.Content(pageContentHtml));

            GamePageDTO gamePage = new()
            {
                URL = uri,
                Name = GetNormalizeName(document),
                Thumbnail = GetThumbnail(document),
                PublishDate = GetDateTime(document),
                CoverLink = GetGameCover(document),
                Summery = GetSummery(document),
                GalleryLink = GetGalleryLink(document),
                DownloadLink = GetDownloadLink(document)
            };
            yield return gamePage;
        }
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
        return new Uri(System.Web.HttpUtility.UrlDecode(hrefElement.Attributes["href"]?.Value));
    }
    private DateOnly GetDateTime(IDocument node)
    {
        var clockNode = node.QuerySelector(".icon-days").NextSibling;
        var stringDatetime = clockNode.TextContent.Split(' ').Select(x => x.Trim()).ToArray();
        stringDatetime[1] = ConstContainer.PersianMonths.First(x => x.Key.Equals(stringDatetime[1])).Value.ToString();
        var intDatetime = stringDatetime.Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray();
        return new DateOnly(intDatetime[2], intDatetime[1], intDatetime[0], new PersianCalendar());
    }
    private string GetNormalizeName(IDocument node)
    {
        var postHeaderNode = node.QuerySelector(".post-content > h2") ?? throw new Exception();
        var rawName = postHeaderNode.TextContent;
        var gameName = RegexHelper.TakeEnglishCharacterAndNumberFromPersian(rawName);
        gameName = RegexHelper.ConvertRomanNumberToEnglish_Under40(gameName);
        return NameNomalize(gameName);
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
        var encoderName = text.Replace(document.Children[1].InnerHtml, "").Trim();
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
    private string GetNameFromURL(Uri url)
    {
        var splitedUrl = url.ToString().Split('/').ToArray();
        var title = splitedUrl.ElementAt(splitedUrl.Length - 2);
        title = title.Replace('-', ' ');
        var result = GetGameNameFromURL().Match(title);
        if (result.Groups.Count > 2)
            throw new Exception();
        var gameName = RegexHelper.ConvertRomanNumberToEnglish_Under40(result.Groups[1].Value);
        return NameNomalize(gameName);
    }
    private string NameNomalize(string name)
    {
        var normalizedName = name.Humanize(LetterCasing.Title);
        normalizedName= normalizedName.Replace(" S ", " ");
        var splitedName = name.Split('’');
        if (splitedName.Length > 1)
        {
            for (int i = 0; i < splitedName.Length; i += 2)
                normalizedName = normalizedName.Replace(splitedName[i], splitedName[i] + "’s");
        }
        return normalizedName;
    }

    [GeneratedRegex("^download ([A-Za-z0-9’ ]*) for pc$")]
    private static partial Regex GetGameNameFromURL();
    private Uri GetThumbnail(IDocument document)
    {
        var postHeaderNode = document.QuerySelector(".review > div.pic > img") ?? throw new Exception();
        var link = System.Web.HttpUtility.UrlDecode(postHeaderNode.Attributes["data-src"]?.Value);
        return new Uri(link!);
    }
}
