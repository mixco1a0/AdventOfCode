using AoC.Solid.Core;
using AoC.Solid.Core.Interfaces;
using AoC.Solid.Services;
using AoC.Solid.Solutions.Y2015;

namespace AoC.Tests.Y2015;

public class Day01Tests
{
#region Part One
    [Fact]
    public void PartOneTest1()
    {
        const string rawInput = @"(())";
        const string expectedOutput = "0";

        IInputProvider inputProvider = new InlineInputProvider(rawInput);
        Day01Solution day01Solution = new();
        string output = day01Solution.SolvePart1(inputProvider);

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void PartOneTest2()
    {
        const string rawInput = @"()()";
        const string expectedOutput = "0";
        
        IInputProvider inputProvider = new InlineInputProvider(rawInput);
        Day01Solution day01Solution = new();
        string output = day01Solution.SolvePart1(inputProvider);

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void PartOneTest3()
    {
        const string rawInput = @"(((";
        const string expectedOutput = "3";
        
        IInputProvider inputProvider = new InlineInputProvider(rawInput);
        Day01Solution day01Solution = new();
        string output = day01Solution.SolvePart1(inputProvider);

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void PartOneTest4()
    {
        const string rawInput = @"(((";
        const string expectedOutput = "3";
        
        IInputProvider inputProvider = new InlineInputProvider(rawInput);
        Day01Solution day01Solution = new();
        string output = day01Solution.SolvePart1(inputProvider);

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void PartOneTest5()
    {
        const string rawInput = @")))";
        const string expectedOutput = "-3";
        
        IInputProvider inputProvider = new InlineInputProvider(rawInput);
        Day01Solution day01Solution = new();
        string output = day01Solution.SolvePart1(inputProvider);

        Assert.Equal(expectedOutput, output);
    }
    
    [Fact]
    public void PartOneInput()
    {
        const string expectedOutput = "74";

        IInputProvider inputProvider = InputService.GetInputProvider(2015, 1);
        Day01Solution day01Solution = new();
        string output = day01Solution.SolvePart1(inputProvider);

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
        Day01Solution day01Solution = new();
        string output = day01Solution.SolvePart2(inputProvider);

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void PartTwoTest2()
    {
        const string rawInput = @"()())";
        const string expectedOutput = "5";
        
        IInputProvider inputProvider = new InlineInputProvider(rawInput);
        Day01Solution day01Solution = new();
        string output = day01Solution.SolvePart2(inputProvider);

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void PartTwoInput()
    {
        const string expectedOutput = "1795";

        IInputProvider inputProvider = InputService.GetInputProvider(2015, 1);
        Day01Solution day01Solution = new();
        string output = day01Solution.SolvePart2(inputProvider);

        Assert.Equal(expectedOutput, output);
    }
#endregion
}