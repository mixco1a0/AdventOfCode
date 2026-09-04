
using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Solid.Core.Input;
using AoC.Solid.Core.Interfaces;
using AoC.Solid.Utils.Interfaces;
using AoC.Solid.Utils.Math;

namespace AoC.Solid.Utils.Math2D;

public record Vec2(int X, int Y) : IVec<Vec2, int>, IComparable<Vec2>, IComparable
{
    public Vec2() : this(Zero.X, Zero.Y) { }

    public static Vec2 Zero => new(0, 0);
    public static Vec2 One => new(1, 1);

    public int Manhattan(Vec2 other)
    {
        return System.Math.Abs(X - other.X) + System.Math.Abs(Y - other.Y);
    }

    public static Vec2 Parse(string input, string delimeters)
    {
        IEnumerable<int> ints = Int.Split(input, delimeters);
        return new(ints.First(), ints.Skip(1).First());
    }

    public static Vec2 operator +(Vec2 left, Vec2 right)
    {
        return new(left.X + right.X, left.Y + right.Y);
    }

    public static Vec2 operator -(Vec2 left, Vec2 right)
    {
        return new(left.X - right.X, left.Y - right.Y);
    }

    public static Vec2 operator *(Vec2 left, int right)
    {
        return new(left.X * right, left.Y * right);
    }

    public static Vec2 operator /(Vec2 left, int right)
    {
        if (right == 0)
        {
            return new();
        }
        return new(left.X / right, left.Y / right);
    }

    public static Vec2 operator %(Vec2 left, int right)
    {
        if (right == 0)
        {
            return new();
        }
        return new(Int.Mod(left.X, right), Int.Mod(left.Y, right));
    }

    public int CompareTo(object? obj)
    {
        if (obj is not Vec2 otherAsVec)
        {
            return -1;
        }
        return otherAsVec.CompareTo(obj);
    }

    public int CompareTo(Vec2? other)
    {
        if (other == null)
        {
            return -1;
        }
        
        int xCompare = X.CompareTo(other.X);
        int yCompare = Y.CompareTo(other.Y);
        return xCompare != 0 ? xCompare : yCompare;
    }
}

internal class Vec2InputProvider(IStringInputProvider stringInputProvider, string delimeters, Func<string, string, Vec2> parseFunc)
    : ObjectInputProvider<Vec2>(stringInputProvider, delimeters, parseFunc);