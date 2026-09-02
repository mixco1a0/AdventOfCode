using System;
using System.Collections.Generic;
using System.IO;

namespace AoC.Solid.Utils;

public static class File
{
    public static string GetInputFilePath(string basePath, int year, int day)
    {
        return Path.Combine(basePath, year.ToString(), day.ToString("D2"));
    }

    public static string GetInputFileName(string path)
    {
        return Path.Combine(path, "in.txt");
    }
    public static string GetOutputFilePath(string basePath, int year, int day)
    {
        return Path.Combine(basePath, year.ToString(), day.ToString("D2"));
    }

    public static string GetOutputFileName(string path, int part)
    {
        return Path.Combine(path, $"p{part:D1}.txt");
    }
}