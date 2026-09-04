using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Solid.Core.Input;
using AoC.Solid.Core.Interfaces;
using AoC.Solid.Utils.Interfaces;
using AoC.Solid.Utils.Math;

namespace AoC.Solid.Utils.Math3D;

public record Vec3(int X, int Y, int Z) : IVec<Vec3, int>
{
    public Vec3() : this(Zero.X, Zero.Y, Zero.Z) { }

    public static Vec3 Zero => new(0, 0, 0);
    public static Vec3 One => new(1, 1, 1);

    public int Manhattan(Vec3 other)
    {
        throw new NotImplementedException();
    }

    public static Vec3 Parse(string input, string delimeters)
    {
        IEnumerable<int> ints = Int.Split(input, delimeters);
        return new(ints.First(), ints.Skip(1).First(), ints.Skip(2).First());
    }

    public static Vec3 operator +(Vec3 left, Vec3 right)
    {
        return new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    public static Vec3 operator -(Vec3 left, Vec3 right)
    {
        return new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    public static Vec3 operator *(Vec3 left, int right)
    {
        return new(left.X * right, left.Y * right, left.Z * right);
    }

    public static Vec3 operator /(Vec3 left, int right)
    {
        if (right == 0)
        {
            return new();
        }
        return new(left.X / right, left.Y / right, left.Z / right);
    }

    public static Vec3 operator %(Vec3 left, int right)
    {
        if (right == 0)
        {
            return new();
        }
        return new(Int.Mod(left.X, right), Int.Mod(left.Y, right), Int.Mod(left.Z, right));
    }
}

internal class Vec3InputProvider(IStringInputProvider stringInputProvider, string delimeters, Func<string, string, Vec3> parseFunc)
    : ObjectInputProvider<Vec3>(stringInputProvider, delimeters, parseFunc);