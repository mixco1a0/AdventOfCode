using System.Collections.Generic;
using System.Linq;

namespace AoC.Solid.Utils.Math;

public static class Int
{
#region int functions
    /// <summary>
    /// Return the positive modulo using provided values
    /// </summary>
    /// <param name="dividend">value to be modded</param>
    /// <param name="divisor">the number value needs to be modded by</param>
    /// <returns></returns>
    public static int Mod(int dividend, int divisor)
    {
        return (dividend % divisor + divisor) % divisor;
    }

    /// <summary>
    /// Call split on a string and then parse it into ints
    /// </summary>
    /// <param name="input">string of separated ints</param>
    /// <param name="seperators">characters used as seperators</param>
    /// <returns></returns>
    public static IEnumerable<int> Split(string input, string seperators)
    {
        string[] split = String.Split(input, seperators);
        return split.Where(s => int.TryParse(s, out int result)).Select(int.Parse);
    }

    /// <summary>
    /// Call split on a string and then parse it into ints
    /// </summary>
    /// <param name="input">string of separated ints</param>
    /// <param name="seperator">character used as seperator</param>
    /// <returns></returns>
    public static IEnumerable<int> Split(string input, char seperator)
    {
        string[] split = String.Split(input, seperator);
        return split.Where(s => int.TryParse(s, out int result)).Select(int.Parse);
    }
#endregion
}