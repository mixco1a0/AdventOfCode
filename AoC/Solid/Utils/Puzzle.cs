using System.Collections.Generic;
using System.IO;

namespace AoC.Solid.Utils;

public static class Puzzle
{
    public static string GetInputFilePath(string basePath, int year, int day)
    {
        return Path.Combine(basePath, year.ToString(), day.ToString("D2"));
    }

    public static IEnumerable<string> ConvertFromInput(string input)
    {
        return String.SplitAndTrim(input, '\n', '\r');
    }

    public static string ConvertToInput(IEnumerable<string> inputs)
    {
        return string.Join('\n', inputs).Trim();
    }
}