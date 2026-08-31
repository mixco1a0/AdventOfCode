using System;

using AoC.Solid.Core.Interfaces;
using AoC.Solid.Services;
using AoC.Solid.Solutions.Y2015;

namespace AoC.Solid.Engine;

internal class EntryPoint
{
    static void Main(string[] args)
    {
        int year = 2015;
        int day = 1;

        IInputProvider inputProvider = InputService.GetInputProvider(year, day);
        Console.WriteLine($"| Running {year}.{day:D2}");
        Day01Solution day01Solution = new();
        string p1 = day01Solution.SolvePart1(inputProvider);
        Console.WriteLine($"| {year}.{day:D2}.p1={p1}");
        string p2 = day01Solution.SolvePart2(inputProvider);
        Console.WriteLine($"| {year}.{day:D2}.p2={p2}");
    }
}