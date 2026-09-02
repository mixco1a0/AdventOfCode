using System.Collections.Generic;
using AoC.Solid.Core.Interfaces;
using AoC.Solid.Utils;

namespace AoC.Solid.Core;

public class InlineInputProvider(string inlineInput) : IStringInputProvider
{
    private readonly string _inlineInput = inlineInput;

    public IEnumerable<string> GetInput()
    {
        return Input.ConvertFromInput(_inlineInput);
    }
}