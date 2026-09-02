using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Solid.Utils;


public static class String
{
    public static IEnumerable<string> SplitAndTrim(string input, char seperator, char trim)
    {
        if (string.IsNullOrEmpty(input))
        {
            return [];
        }

        return input.Split(seperator).Select(str => str.Trim(trim));
    }

    public static string[] Split(string input, char seperator)
    {
        return input.Split(seperator, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    public static string[] Split(string input, string seperator)
    {
        return input.Split(seperator.ToCharArray(), StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }
}