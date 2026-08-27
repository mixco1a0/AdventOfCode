using System.Collections.Generic;
using System.Linq;
using AoC.Solid.Core.Interfaces;

namespace AoC.Solid.Core;

public class InlineInputProvider(string inlineInput) : IInputProvider
{
    private readonly string _inlineInput = inlineInput;

    public IEnumerable<string> GetInput()
    {
        if (string.IsNullOrEmpty(_inlineInput))
        {
            return [];
        }

        return _inlineInput.Split('\n').Select(str => str.Trim('\r'));
    }
}