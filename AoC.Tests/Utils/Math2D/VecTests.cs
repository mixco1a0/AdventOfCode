using AoC.Solid.Utils.Math2D;

namespace AoC.Tests.Utils.Math2D;

public class VecTests
{
    [Fact]
    public void ManhattenAgainstZero()
    {
        int expectedManhattan = 5;
        Vec2 left = new(2, 3);
        int manhattan = left.Manhattan(Vec2.Zero);

        Assert.Equal(expectedManhattan, manhattan);
    }
    
    [Fact]
    public void ManhattenAgainstOne()
    {
        int expectedManhattan = 9;
        Vec2 left = new(5, 6);
        int manhattan = left.Manhattan(Vec2.One);

        Assert.Equal(expectedManhattan, manhattan);
    }
    
    [Fact]
    public void ManhattenAgainstPositive()
    {
        int expectedManhattan = 20;
        Vec2 left = new(1, 2);
        Vec2 right = new(11, 12);
        int manhattan = left.Manhattan(right);

        Assert.Equal(expectedManhattan, manhattan);
    }
    
    [Fact]
    public void ManhattenAgainstNegative()
    {
        int expectedManhattan = 11;
        Vec2 left = new(-2, -1);
        Vec2 right = new(-6, -8);
        int manhattan = left.Manhattan(right);

        Assert.Equal(expectedManhattan, manhattan);
    }
    
    [Fact]
    public void ManhattenWideRange()
    {
        int expectedManhattan = 23;
        Vec2 left = new(3, 6);
        Vec2 right = new(-6, -8);
        int manhattan = left.Manhattan(right);

        Assert.Equal(expectedManhattan, manhattan);
    }
    
    [Fact]
    public void Addition()
    {
        Vec2 expected = new(4, 6);
        Vec2 left = new(1, 2);
        Vec2 right = new(3, 4);
        Vec2 output = left + right;

        Assert.Equal(expected, output);
    }
    
    [Fact]
    public void Subtraction()
    {
        Vec2 expected = new(-2, -2);
        Vec2 left = new(1, 2);
        Vec2 right = new(3, 4);
        Vec2 output = left - right;

        Assert.Equal(expected, output);
    }
    
    [Fact]
    public void Multiplication()
    {
        Vec2 expected = new(-1, -2);
        Vec2 left = new(1, 2);
        int right = -1;
        Vec2 output = left * right;

        Assert.Equal(expected, output);
    }
    
    [Fact]
    public void Parse()
    {
        Vec2 expected = new(5, 7);
        string input = "5,7";
        string delimeters = ",";
        Vec2 output = Vec2.Parse(input, delimeters);

        Assert.Equal(expected, output);
    }
    
    [Fact]
    public void Division()
    {
        Vec2 expected = new(5, 10);
        Vec2 left = new(10, 20);
        int right = 2;
        Vec2 output = left / right;

        Assert.Equal(expected, output);
    }
    
    [Fact]
    public void Modulo()
    {
        Vec2 expected = new(7, 5);
        Vec2 left = new(23, 13);
        int right = 8;
        Vec2 output = left % right;

        Assert.Equal(expected, output);
    }
}