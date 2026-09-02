using AoC.Solid.Core;
using AoC.Solid.Core.Interfaces;
using AoC.Solid.Services;
using AoC.Solid.Solutions.Y2015;

namespace AoC.Tests.Y2015;

public class Day01Tests
{
    private readonly int _day = 1;
    private readonly int _year = 2015;

#region Part One
    [Fact]
    public void PartOneTest1()
    {
        const string rawInput = @"(())";
        const string expectedOutput = "0";

        IInputProvider inputProvider = new InlineInputProvider(rawInput);
        Day01Solution daySolution = new();
        string output = daySolution.SolvePart1(inputProvider);

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void PartOneTest2()
    {
        const string rawInput = @"()()";
        const string expectedOutput = "0";
        
        IInputProvider inputProvider = new InlineInputProvider(rawInput);
        Day01Solution daySolution = new();
        string output = daySolution.SolvePart1(inputProvider);

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void PartOneTest3()
    {
        const string rawInput = @"(((";
        const string expectedOutput = "3";
        
        IInputProvider inputProvider = new InlineInputProvider(rawInput);
        Day01Solution daySolution = new();
        string output = daySolution.SolvePart1(inputProvider);

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void PartOneTest4()
    {
        const string rawInput = @"(((";
        const string expectedOutput = "3";
        
        IInputProvider inputProvider = new InlineInputProvider(rawInput);
        Day01Solution daySolution = new();
        string output = daySolution.SolvePart1(inputProvider);

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void PartOneTest5()
    {
        const string rawInput = @")))";
        const string expectedOutput = "-3";
        
        IInputProvider inputProvider = new InlineInputProvider(rawInput);
        Day01Solution daySolution = new();
        string output = daySolution.SolvePart1(inputProvider);

        Assert.Equal(expectedOutput, output);
    }
    
    [Fact]
    public void PartOneInput()
    {
        IInputProvider inputProvider = InputService.GetInputProvider(_year, _day);
        Day01Solution daySolution = new();
        string output = daySolution.SolvePart1(inputProvider);
        
        string expectedOutput = OutputService.GetOutput(_year, _day, 1);
        Assert.False(string.IsNullOrEmpty(expectedOutput));

        Assert.Equal(expectedOutput, output);
    }
#endregion

#region Part Two
    [Fact]
    public void PartTwoTest1()
    {
        const string rawInput = @")";
        const string expectedOutput = "1";
        
        IInputProvider inputProvider = new InlineInputProvider(rawInput);
        Day01Solution daySolution = new();
        string output = daySolution.SolvePart2(inputProvider);

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void PartTwoTest2()
    {
        const string rawInput = @"()())";
        const string expectedOutput = "5";
        
        IInputProvider inputProvider = new InlineInputProvider(rawInput);
        Day01Solution daySolution = new();
        string output = daySolution.SolvePart2(inputProvider);

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void PartTwoInput()
    {
        IInputProvider inputProvider = InputService.GetInputProvider(_year, _day);
        Day01Solution daySolution = new();
        string output = daySolution.SolvePart2(inputProvider);
        
        string expectedOutput = OutputService.GetOutput(_year, _day, 2);
        Assert.False(string.IsNullOrEmpty(expectedOutput));

        Assert.Equal(expectedOutput, output);
    }
#endregion
}