using System.IO;
using AoC.Solid.Core.Interfaces;

namespace AoC.Solid.Core;

public class InputFileWriter(string path, int year, int day) : IWriter
{
    private readonly string _path = path;
    private readonly int _year = year;
    private readonly int _day = day;

    public void Write(IInputProvider inputProvider)
    {
        string path = Utils.Puzzle.GetInputFilePath(_path, _year, _day);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string filename = Utils.Puzzle.GetInputFileName(path);
        string contents = Utils.Puzzle.ConvertToInput(inputProvider.GetInput());

        using StreamWriter streamWriter = File.AppendText(filename);
        streamWriter.Write(contents);
    }
}