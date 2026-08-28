using AoC.Solid.Core.Interfaces;
using AoC.Solid.Core;
using AoC.Solid.Solutions.Y2015;
using System.Threading.Tasks;
using System;

namespace AoC.Solid.Engine;

internal class EntryPoint
{
    static void Main(string[] args)
    {
        // check if input file exists
        // if it doesn't, try to download it from aoc.com
        // grab input
        // test only
        DownloadInputProvider downloadInputProvider = new(2015, 1);
        downloadInputProvider.DownloadInput();

        Console.WriteLine("reading input file....");
        string[] allS = [.. downloadInputProvider.GetInput()];
        foreach (string s in allS)
        {
            Console.WriteLine($"\t{s}");
        }

        Day01Solution day01Solution = new();
        //IInputProvider inputProvider = new InlineInputProvider(@"(())");
        string p1 = day01Solution.SolvePart1(downloadInputProvider);
        string p2 = day01Solution.SolvePart2(downloadInputProvider);
    }
}