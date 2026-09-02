using System.Collections.Generic;

namespace AoC.Solid.Utils;

public static class Input
{
    public static IEnumerable<string> ConvertFromInput(string input)
    {
        return String.SplitAndTrim(input, '\n', '\r');
    }

    public static string ConvertToInput(IEnumerable<string> inputs)
    {
        return string.Join('\n', inputs).Trim();
    }
}