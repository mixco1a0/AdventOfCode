using AoC.Solid.Core;
using AoC.Solid.Core.Interfaces;
using AoC.Solid.Services;
using AoC.Solid.Solutions.Y2015;

namespace AoC.Tests.Y2015;

public class Day02Tests
{
    private readonly int _day = 2;
    private readonly int _year = 2015;

#region Part One
    [Fact]
    public void PartOneTest1()
    {
        const string rawInput = @"2x3x4";
        const string expectedOutput = "58";

        IStringInputProvider stringInputProvider = new InlineInputProvider(rawInput);
        Day02Solution daySolution = new();
        string output = daySolution.SolvePart1(stringInputProvider);

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void PartOneTest2()
    {
        const string rawInput = @"1x1x10";
        const string expectedOutput = "43";

        IStringInputProvider stringInputProvider = new InlineInputProvider(rawInput);
        Day02Solution daySolution = new();
        string output = daySolution.SolvePart1(stringInputProvider);

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void PartOneInput()
    {
        IStringInputProvider stringInputProvider = InputService.GetInputProvider(_year, _day);
        Day02Solution daySolution = new();
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
        const string rawInput = @"2x3x4";
        const string expectedOutput = "34";

        IStringInputProvider stringInputProvider = new InlineInputProvider(rawInput);
        Day02Solution daySolution = new();
        string output = daySolution.SolvePart2(stringInputProvider);

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void PartTwoTest2()
    {
        const string rawInput = @"1x1x10";
        const string expectedOutput = "14";

        IStringInputProvider stringInputProvider = new InlineInputProvider(rawInput);
        Day02Solution daySolution = new();
        string output = daySolution.SolvePart2(stringInputProvider);

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void PartTwoInput()
    {
        IStringInputProvider stringInputProvider = InputService.GetInputProvider(_year, _day);
        Day02Solution daySolution = new();
        string output = daySolution.SolvePart2(stringInputProvider);

        string expectedOutput = OutputService.GetOutput(_year, _day, 2);
        Assert.False(string.IsNullOrEmpty(expectedOutput));

        Assert.Equal(expectedOutput, output);
    }
#endregion
}