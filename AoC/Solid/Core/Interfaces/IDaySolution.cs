namespace AoC.Solid.Core.Interfaces;

public interface IDaySolution
{
    int Version { get; }
    int Day { get; }
    int Year { get; }

    string SolvePart1(IStringInputProvider stringInputProvider);
    string SolvePart2(IStringInputProvider stringInputProvider);
}