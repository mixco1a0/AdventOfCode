using System;

using AoC.Solid.Core.Interfaces;
using AoC.Solid.Services;

namespace AoC.Solid.Engine;

internal class EntryPoint
{
    static void Main(string[] args)
    {
        bool writeOutputFiles = true;
        int year = 2015;
        int day = 3;

        IStringInputProvider stringInputProvider = InputService.GetInputProvider(year, day);
        IDaySolution? daySolution = DayService.GetDaySolution(year, day);
        if (daySolution != null)
        {
            Console.WriteLine($"| Running {year}.{day:D2}");
            string p1 = daySolution.SolvePart1(stringInputProvider);
            if (writeOutputFiles)
            {
                OutputService.SetOutput(year, day, 1, p1);
            }
            Console.WriteLine($"| {year}.{day:D2}.p1={p1}");
            string p2 = daySolution.SolvePart2(stringInputProvider);
            if (writeOutputFiles)
            {
                OutputService.SetOutput(year, day, 2, p2);
            }
            Console.WriteLine($"| {year}.{day:D2}.p2={p2}");
        }
    }
}