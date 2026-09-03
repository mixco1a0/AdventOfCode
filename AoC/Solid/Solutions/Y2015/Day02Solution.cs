using System.Collections.Generic;
using AoC.Solid.Core.Interfaces;
using AoC.Solid.Utils.Math;

namespace AoC.Solid.Solutions.Y2015;

public class Day02Solution : IDaySolution
{
    public int Version => 1;
    public int Day => 2;
    public int Year => 2015;

    private Vec3 Parse(string input)
    {
        List<int> ints = [.. Number.Split(input, 'x')];
        ints.Sort();
        return new(ints[0], ints[1], ints[2]);
    }

    public string SolvePart1(IStringInputProvider stringInputProvider)
    {
        Vec3InputProvider vec3InputProvider = new(stringInputProvider, Parse);
        IEnumerable<Vec3> vec3s = vec3InputProvider.GetInput();

        int total = 0;
        foreach (Vec3 vec3 in vec3s)
        {
            total += 3 * vec3.X * vec3.Y + 2 * vec3.X * vec3.Z + 2 * vec3.Y * vec3.Z;
        }
        return total.ToString();
    }

    public string SolvePart2(IStringInputProvider stringInputProvider)
    {
        Vec3InputProvider vec3InputProvider = new(stringInputProvider, Parse);
        IEnumerable<Vec3> vec3s = vec3InputProvider.GetInput();

        int total = 0;
        foreach (Vec3 vec3 in vec3s)
        {
            total += 2 * vec3.X + 2 * vec3.Y + vec3.X * vec3.Y * vec3.Z;
        }
        return total.ToString();
    }
}