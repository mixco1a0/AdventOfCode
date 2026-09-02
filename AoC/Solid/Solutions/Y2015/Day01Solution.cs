using System.Linq;
using AoC.Solid.Core.Interfaces;

namespace AoC.Solid.Solutions.Y2015;

public class Day01Solution : IDaySolution
{
    public int Version => 1;
    public int Day => 1;
    public int Year => 2015;

    private readonly char _open = '(';
    private readonly char _close = ')';

    public string SolvePart1(IStringInputProvider stringInputProvider)
    {
        string firstLine = stringInputProvider.GetInput().First();
        return (firstLine.Count(c => c == _open) - firstLine.Count(c => c == _close)).ToString();
    }

    public string SolvePart2(IStringInputProvider stringInputProvider)
    {
        string firstLine = stringInputProvider.GetInput().First();
        int curFloor = 0;
        for (int i = 0; i < firstLine.Length; ++i)
        {
            if (firstLine[i] == _open)
            {
                ++curFloor;
            }
            else if (firstLine[i] == _close)
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