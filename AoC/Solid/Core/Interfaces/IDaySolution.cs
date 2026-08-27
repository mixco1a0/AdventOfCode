namespace AoC.Solid.Core.Interfaces;

public interface IDaySolution
{
    public int Day { get; }
    public int Year { get; }

    public string SolvePart1(IInputProvider inputProvider);
    public string SolvePart2(IInputProvider inputProvider);
}