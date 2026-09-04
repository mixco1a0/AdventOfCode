using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Solid.Core.Interfaces;
using AoC.Solid.Utils.Math;

namespace AoC.Solid.Utils.Math3D;

public record Vec3(int X, int Y, int Z);

internal class Vec3InputProvider(IStringInputProvider stringInputProvider, Func<string, Vec3> parseFunc) : IInputProvider<Vec3>
{
    private readonly IStringInputProvider _stringInputProvider = stringInputProvider;
    private readonly Func<string, Vec3> _parseFunc = parseFunc ?? DefaultParser;
    private IEnumerable<Vec3> _vec3 = [];

    public IEnumerable<Vec3> GetInput()
    {
        if (!_vec3.Any())
        {
            _vec3 = _stringInputProvider.GetInput().Select(s => _parseFunc(s));
        }
        return _vec3;
    }

    private static Vec3 DefaultParser(string input)
    {
        IEnumerable<int> ints = Number.Split(input, ',');
        return new(ints.First(), ints.Skip(1).First(), ints.Skip(2).First());
    }
}