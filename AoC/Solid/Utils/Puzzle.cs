using System;

namespace AoC.Solid.Utils;

public static class Puzzle
{
    public static string DataDirectory => "AOC_DATA";
    public static string SessionKey => "AOC_SESSION";

    public static string GetEnvironmentVariable(string variableName)
    {
        string? variableValue = Environment.GetEnvironmentVariable(variableName);
        if (string.IsNullOrEmpty(variableValue))
        {
            throw new Exception($"Environment variable \"{variableName}\" was not found.");
        }
        return variableValue;
    }
}