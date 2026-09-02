using System.IO;
using AoC.Solid.Core;
using AoC.Solid.Core.Interfaces;
using AoC.Solid.Utils;

namespace AoC.Solid.Services;

public class InputService
{
    private static readonly string _dataDirectory = Puzzle.GetEnvironmentVariable(Puzzle.DataDirectory);

    public static IStringInputProvider GetInputProvider(int year, int day)
    {
        IStringInputProvider inputProvider;
        InputFileReader inputFileReader = new(_dataDirectory, year, day);
        try
        {
            inputProvider = inputFileReader.Read();
        }
        catch (FileNotFoundException)
        {
            // download the input
            DownloadInputProvider downloadInputProvider = new(year, day);
            downloadInputProvider.DownloadInput();
            inputProvider = downloadInputProvider;

            // write out the file
            InputFileWriter inputFileWriter = new(_dataDirectory, year, day);
            inputFileWriter.Write(inputProvider);
        }
        return inputProvider;
    }
}