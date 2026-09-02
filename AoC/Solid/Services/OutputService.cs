using AoC.Solid.Core;
using AoC.Solid.Utils;

namespace AoC.Solid.Services;

public class OutputService
{
    private static readonly string _dataDirectory = Puzzle.GetEnvironmentVariable(Puzzle.DataDirectory);

    public static string GetOutput(int year, int day, int part)
    {
        string output = string.Empty;
        OutputFileReader outputFileReader = new(_dataDirectory, year, day, part);
        try
        {
            output = outputFileReader.Read();
        }
        catch
        {
            output = string.Empty;
        }
        return output;
    }

    public static void SetOutput(int year, int day, int part, string output)
    {
        OutputFileWriter outputFileWriter = new(_dataDirectory, year, day, part);
        outputFileWriter.Write(output);
    }
}