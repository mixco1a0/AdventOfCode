using System.Collections.Generic;
using System.Linq;
using AoC.Solid.Core.Interfaces;

namespace AoC.Solid.Solutions.Y2015;

public class Day01Solution : IDaySolution
{
    public int Day => 1;
    public int Year => 2015;

    private static char Open => '(';
    private static char Close => ')';

    public string SolvePart1(IInputProvider inputProvider)
    {
        IEnumerable<string> input = inputProvider.GetInput();
        string oneLine = input.First();
        return (oneLine.Count(c => c == Open) - oneLine.Count(c => c == Close)).ToString();
    }

    public string SolvePart2(IInputProvider inputProvider)
    {
        IEnumerable<string> input = inputProvider.GetInput();
        string oneLine = input.First();
        int curFloor = 0;
        for (int i = 0; i < oneLine.Length; ++i)
        {
            if (oneLine[i] == Open)
            {
                ++curFloor;
            }
            else if (oneLine[i] == Close)
            {
                --curFloor;
            }

            if (curFloor < 0)
            {
                return (i + 1).ToString();
            }
        }
        return string.Empty;
    }
}