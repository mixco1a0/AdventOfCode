using System.Transactions;
using AoC.Solid.Core;
using AoC.Solid.Core.Interfaces;
using AoC.Solid.Services;
using AoC.Solid.Utils.Math;

namespace AoC.Tests.Math;

public class IntTests
{
    [Fact]
    public void ModInRange()
    {
        int actualMod = 2;
        int dividend = 2;
        int divisor = 8;
        int mod = Int.Mod(dividend, divisor);

        Assert.Equal(actualMod, mod);
    }
    
    [Fact]
    public void ModAboveRange()
    {
        int actualMod = 3;
        int dividend = 9;
        int divisor = 6;
        int mod = Int.Mod(dividend, divisor);

        Assert.Equal(actualMod, mod);
    }
    
    [Fact]
    public void ModBelowRange()
    {
        int actualMod = 1;
        int dividend = -3;
        int divisor = 4;
        int mod = Int.Mod(dividend, divisor);

        Assert.Equal(actualMod, mod);
    }
    
    [Fact]
    public void SplitCharEmpty()
    {
        string input = "";
        char seperator = ' ';
        IEnumerable<int> split = Int.Split(input, seperator);

        Assert.False(split.Any());
    }
    
    [Fact]
    public void SplitCharWrongDelimeter()
    {
        string input = "1,2,3";
        char seperator = ':';
        IEnumerable<int> split = Int.Split(input, seperator);

        Assert.False(split.Any());
    }
    
    [Fact]
    public void SplitCharWithDelimeter()
    {
        int[] expectedSplit = [1, 2, 3];
        string input = "1,2,3";
        char seperator = ',';
        IEnumerable<int> split = Int.Split(input, seperator);

        Assert.True(expectedSplit.SequenceEqual(split));
    }
    
    [Fact]
    public void SplitStringEmpty()
    {
        string input = "";
        string seperators = "";
        IEnumerable<int> split = Int.Split(input, seperators);

        Assert.False(split.Any());
    }
    
    [Fact]
    public void SplitStringWrongDelimeters()
    {
        string input = "1,2,3";
        string seperators = ":+";
        IEnumerable<int> split = Int.Split(input, seperators);

        Assert.False(split.Any());
    }
    
    [Fact]
    public void SplitStringWithSingleDelimeter()
    {
        int[] expectedSplit = [1, 2, 3];
        string input = "1,2,3";
        string seperators = ",";
        IEnumerable<int> split = Int.Split(input, seperators);

        Assert.True(expectedSplit.SequenceEqual(split));
    }
    
    [Fact]
    public void SplitStringWithMultipleDelimeters()
    {
        int[] expectedSplit = [1, 2, 3];
        string input = "1:2,3";
        string seperators = ",:";
        IEnumerable<int> split = Int.Split(input, seperators);

        Assert.True(expectedSplit.SequenceEqual(split));
    }
}

