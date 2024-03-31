using Humanizer;
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
    [GeneratedRegex("^X{0,3}(IX|IV|V?I{0,3})$")]
    private static partial Regex RomanNumber_Under40Pattern();

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
    public static string ConvertRomanNumberToEnglish_Under40(string input)
    {
        string gameName = string.Empty;
        input = input.ToUpper();
        foreach (var item in input.Split(' '))
        {
            if (IsRomanNumber_Under40(item))
                gameName += item.FromRoman().ToString() + " ";
            else gameName += item + " ";
        }
        return gameName.TrimEnd();
    }
    public static bool IsRomanNumber_Under40(string input)
    {
        var result = RomanNumber_Under40Pattern().Match(input.ToUpper());
        return !string.IsNullOrEmpty(result.Value);
    }
}
