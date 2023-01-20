using System.Linq;

namespace GuildManager.Utils;

public static class StringExtensions
{
    public static int? ExtractInteger(this string str)
    {
        var success = int.TryParse(new string(str.Where(char.IsDigit).ToArray()), out var result);
        return success ? result : null;
    }
}