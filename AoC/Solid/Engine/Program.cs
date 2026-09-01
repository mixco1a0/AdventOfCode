using System;

using AoC.Solid.Core.Interfaces;
using AoC.Solid.Services;

namespace AoC.Solid.Engine;

internal class EntryPoint
{
    static void Main(string[] args)
    {
        int year = 2015;
        int day = 1;

        IInputProvider inputProvider = InputService.GetInputProvider(year, day);
        IDaySolution? daySolution = DayService.GetDaySolution(year, day);
        if (daySolution != null)
        {
            Console.WriteLine($"| Running {year}.{day:D2}");
            string p1 = daySolution.SolvePart1(inputProvider);
            Console.WriteLine($"| {year}.{day:D2}.p1={p1}");
            string p2 = daySolution.SolvePart2(inputProvider);
            Console.WriteLine($"| {year}.{day:D2}.p2={p2}");
        }
    }
}