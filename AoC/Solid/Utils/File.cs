using System.IO;

namespace AoC.Solid.Utils;

public static class File
{
    private static string GetDefaultFilePath(string basePath, int year, int day)
    {
        return Path.Combine(basePath, $"Year{year:D4}", $"Day{day:D2}");
    }

    public static string GetInputFilePath(string basePath, int year, int day)
    {
        return GetDefaultFilePath(basePath, year, day);
    }

    public static string GetInputFileName(string path)
    {
        return Path.Combine(path, "in.txt");
    }
    public static string GetOutputFilePath(string basePath, int year, int day)
    {
        return GetDefaultFilePath(basePath, year, day);
    }

    public static string GetOutputFileName(string path, int part)
    {
        return Path.Combine(path, $"p{part:D1}.txt");
    }
}