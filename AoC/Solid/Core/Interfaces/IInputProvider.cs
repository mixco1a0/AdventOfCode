using System.Collections.Generic;

namespace AoC.Solid.Core.Interfaces;

public interface IInputProvider
{
    IEnumerable<string> GetInput();
}