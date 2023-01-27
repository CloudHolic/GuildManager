using System;
using System.Linq;

namespace GuildManager.Utils;

public static class StringExtensions
{
    public static int? ExtractInteger(this string str)
    {
        var success = int.TryParse(new string(str.Where(char.IsDigit).ToArray()), out var result);
        return success ? result : null;
    }

    public static string ReplaceLastOccurrence(this string str, string find, string replace)
    {
        var place = str.LastIndexOf(find, StringComparison.Ordinal);

        return place == -1 ? str : str.Remove(place, find.Length).Insert(place, replace);
    }
}