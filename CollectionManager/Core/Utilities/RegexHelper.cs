using System.Text.RegularExpressions;

namespace CollectionManager.Core.Utilities;

public static partial class RegexHelper
{
    [GeneratedRegex(@"\s{2,}")]
    private static partial Regex RemoveWhiteSpacePattern();
    [GeneratedRegex(@"([0-9]|\.)*")]
    private static partial Regex TakeNumberPattern();
    [GeneratedRegex("\".*\"")]
    private static partial Regex TakeBetweenQuotePattern();


    public static string RemoveWhiteSpace(string input)
    {
       return RemoveWhiteSpacePattern().Replace(input,"");
    }
    public static string TakeNumber(string input)
    {
        var temp =RemoveWhiteSpace(input);
        return TakeNumberPattern().Matches(temp).First(x=> !string.IsNullOrWhiteSpace(x.Value)).Value;
    }
    public static string TakeBetweenQuote(string input)
    {
        return TakeBetweenQuotePattern().Matches(input).First().Value.Replace("\"","");
    }
}
