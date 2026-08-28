using System.Collections.Generic;

namespace AoC.Solid.Utils;

public static class Puzzle
{
    public static IEnumerable<string> ConvertInput(string input)
    {
        return String.SplitAndTrim(input, '\n', '\r');
    }
}