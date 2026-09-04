using System.Collections.Generic;
using AoC.Solid.Core.Interfaces;

namespace AoC.Solid.Core.Input;

public class InlineInputProvider(string inlineInput) : IStringInputProvider
{
    private readonly string _inlineInput = inlineInput;

    public IEnumerable<string> GetInput()
    {
        return Utils.Input.ConvertFromInput(_inlineInput);
    }
}