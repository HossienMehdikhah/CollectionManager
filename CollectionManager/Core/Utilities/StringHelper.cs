using System.Text;

namespace CollectionManager.Core.Utilities;

public static class StringHelper
{
    public static string ToCapital(this string str)
    {
        StringBuilder normalizedName = new();
        foreach (var letter in str.Split(' '))
        {
            var temp = letter.Remove(0, 1);
            temp = temp.Insert(0, char.ToUpper(letter.ElementAt(0)).ToString());
            normalizedName.Append(temp);
            normalizedName.Append(' ');
        }
        return normalizedName.ToString().Trim();
    }

    private static Dictionary<char, int> RomanMap = new Dictionary<char, int>()
    {
        {'I', 1},
        {'V', 5},
        {'X', 10},
        {'L', 50},
        {'C', 100},
        {'D', 500},
        {'M', 1000}
    };

    public static string FromRoman(this string src)
    {
        var roman = src.ToUpper();
        int number = 0;
        for (int i = 0; i < roman.Length; i++)
        {
            if (i + 1 < roman.Length && RomanMap[roman[i]] < RomanMap[roman[i + 1]])
            {
                number -= RomanMap[roman[i]];
            }
            else
            {
                number += RomanMap[roman[i]];
            }
        }
        return number.ToString();
    }
}
