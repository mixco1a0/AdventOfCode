using System.Collections.Generic;
using System.Linq;
using AoC.Solid.Core.Interfaces;
using AoC.Solid.Utils;

namespace AoC.Solid.Core;

public class InlineInputProvider(string inlineInput) : IInputProvider
{
    private readonly string _inlineInput = inlineInput;

    public IEnumerable<string> GetInput()
    {
        return Puzzle.ConvertInput(_inlineInput);
    }
}