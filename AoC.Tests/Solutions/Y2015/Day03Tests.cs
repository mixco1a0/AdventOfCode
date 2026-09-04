using AoC.Solid.Core.Input;
using AoC.Solid.Core.Interfaces;
using AoC.Solid.Services;
using AoC.Solid.Solutions.Y2015;

namespace AoC.Tests.Solutions.Y2015;

public class Day03Tests
{
    private readonly int _day = 3;
    private readonly int _year = 2015;

#region Part One
    [Fact]
    public void PartOneTest1()
    {
        const string rawInput = @">";
        const string expectedOutput = "2";

        IStringInputProvider stringInputProvider = new InlineInputProvider(rawInput);
        Day03Solution daySolution = new();
        string output = daySolution.SolvePart1(stringInputProvider);

        Assert.Equal(expectedOutput, output);
    }
    
    [Fact]
    public void PartOneTest2()
    {
        const string rawInput = @"^>v<";
        const string expectedOutput = "4";

        IStringInputProvider stringInputProvider = new InlineInputProvider(rawInput);
        Day03Solution daySolution = new();
        string output = daySolution.SolvePart1(stringInputProvider);

        Assert.Equal(expectedOutput, output);
    }
    
    [Fact]
    public void PartOneTest3()
    {
        const string rawInput = @"^v^v^v^v^v";
        const string expectedOutput = "2";

        IStringInputProvider stringInputProvider = new InlineInputProvider(rawInput);
        Day03Solution daySolution = new();
        string output = daySolution.SolvePart1(stringInputProvider);

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void PartOneInput()
    {
        IStringInputProvider stringInputProvider = InputService.GetInputProvider(_year, _day);
        Day03Solution daySolution = new();
        string output = daySolution.SolvePart1(stringInputProvider);

        string expectedOutput = OutputService.GetOutput(_year, _day, 1);
        Assert.False(string.IsNullOrEmpty(expectedOutput));

        Assert.Equal(expectedOutput, output);
    }
#endregion

#region Part Two
    [Fact]
    public void PartTwoTest1()
    {
        const string rawInput = @"^v";
        const string expectedOutput = "3";

        IStringInputProvider stringInputProvider = new InlineInputProvider(rawInput);
        Day03Solution daySolution = new();
        string output = daySolution.SolvePart2(stringInputProvider);

        Assert.Equal(expectedOutput, output);
    }
    
    [Fact]
    public void PartTwoTest2()
    {
        const string rawInput = @"^>v<";
        const string expectedOutput = "3";

        IStringInputProvider stringInputProvider = new InlineInputProvider(rawInput);
        Day03Solution daySolution = new();
        string output = daySolution.SolvePart2(stringInputProvider);

        Assert.Equal(expectedOutput, output);
    }
    
    [Fact]
    public void PartTwoTest3()
    {
        const string rawInput = @"^v^v^v^v^v";
        const string expectedOutput = "11";

        IStringInputProvider stringInputProvider = new InlineInputProvider(rawInput);
        Day03Solution daySolution = new();
        string output = daySolution.SolvePart2(stringInputProvider);

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void PartTwoInput()
    {
        IStringInputProvider stringInputProvider = InputService.GetInputProvider(_year, _day);
        Day03Solution daySolution = new();
        string output = daySolution.SolvePart2(stringInputProvider);

        string expectedOutput = OutputService.GetOutput(_year, _day, 2);
        Assert.False(string.IsNullOrEmpty(expectedOutput));

        Assert.Equal(expectedOutput, output);
    }
#endregion
}