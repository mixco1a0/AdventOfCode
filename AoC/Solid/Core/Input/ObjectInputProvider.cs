using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Solid.Core.Interfaces;

namespace AoC.Solid.Core.Input;

public class ObjectInputProvider<T>(IStringInputProvider stringInputProvider, string delimeters, Func<string, string, T> parseFunc) : IInputProvider<T>
{
    private readonly IStringInputProvider _stringInputProvider = stringInputProvider;
    private readonly string _delimeters = delimeters;
    private readonly Func<string, string, T> _parseFunc = parseFunc;
    private IEnumerable<T> _objects = [];

    public IEnumerable<T> GetInput()
    {
        if (!_objects.Any())
        {
            _objects = _stringInputProvider.GetInput().Select(s => _parseFunc(s, _delimeters));
        }
        return _objects;
    }
}