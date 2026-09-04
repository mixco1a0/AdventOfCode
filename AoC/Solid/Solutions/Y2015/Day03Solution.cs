using System.Collections.Generic;
using System.Linq;
using AoC.Solid.Core.Interfaces;
using AoC.Solid.Utils.Math;
using AoC.Solid.Utils.Math2D;

namespace AoC.Solid.Solutions.Y2015;

public class Day03Solution : IDaySolution
{
    public int Version => 1;
    public int Day => 3;
    public int Year => 2015;

    private string SharedSolution(IStringInputProvider stringInputProvider, int santaCount)
    {
        string firstLine = stringInputProvider.GetInput().First();
        Vec2[] santa = new Vec2[santaCount];
        for (int i = 0; i < santaCount; ++i)
        {
            santa[i] = new();
        }
        int curSanta = 0;
        HashSet<Vec2> visited = [new()];
        foreach (char c in firstLine)
        {
            Dir dir = Map.SimpleArrowFlipped[c];
            santa[curSanta] += Map.Neighbor[dir];
            visited.Add(santa[curSanta]);
            curSanta = Int.Mod(curSanta + 1, santaCount);
        }
        return visited.Count.ToString();
    }

    public string SolvePart1(IStringInputProvider stringInputProvider)
    {
        return SharedSolution(stringInputProvider, 1);
    }

    public string SolvePart2(IStringInputProvider stringInputProvider)
    {
        return SharedSolution(stringInputProvider, 2);
    }
}