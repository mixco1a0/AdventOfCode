using AoC.Solid.Core.Interfaces;
using AoC.Solid.Core;
using AoC.Solid.Solutions.Y2015;

namespace AoC.Solid.Engine;

internal class EntryPoint
{
    static void Main(string[] args)
    {
        Day01Solution day01Solution = new();
        IInputProvider inputProvider = new InlineInputProvider(@"(())");
        string p1 = day01Solution.SolvePart1(inputProvider);
        string p2 = day01Solution.SolvePart2(inputProvider);
    }
}