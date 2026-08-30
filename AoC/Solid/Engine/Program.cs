using AoC.Solid.Core.Interfaces;
using AoC.Solid.Core;
using AoC.Solid.Solutions.Y2015;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;

namespace AoC.Solid.Engine;

internal class EntryPoint
{
    static void Main(string[] args)
    {
        string dataPath = @"D:\Workspace\data\aoc";
        int year = 2015;
        int day = 1;

        IInputProvider inputProvider;
        InputFileReader inputFileReader = new(dataPath, year, day);
        string inputFileContents = string.Empty;
        try
        {
            Console.WriteLine($"| Reading from file");
            inputProvider = inputFileReader.Read();
        }
        catch (FileNotFoundException)
        {
            // download the input
            Console.WriteLine($"| Downloading input");
            DownloadInputProvider downloadInputProvider = new(year, day);
            downloadInputProvider.DownloadInput();
            inputProvider = downloadInputProvider;

            // write out the file
            Console.WriteLine($"| Writing to file");
            InputFileWriter inputFileWriter = new(dataPath, year, day);
            inputFileWriter.Write(inputProvider);
        }

        Console.WriteLine($"| Running {year}.{day:D2}");
        Day01Solution day01Solution = new();
        string p1 = day01Solution.SolvePart1(inputProvider);
        Console.WriteLine($"| {year}.{day:D2}.p1={p1}");
        string p2 = day01Solution.SolvePart2(inputProvider);
        Console.WriteLine($"| {year}.{day:D2}.p2={p2}");
    }
}