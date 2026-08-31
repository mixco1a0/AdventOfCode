using System.IO;
using AoC.Solid.Core.Interfaces;
using AoC.Solid.Utils;

namespace AoC.Solid.Core;

public class InputFileWriter(string path, int year, int day) : IInputWriter
{
    private readonly string _path = path;
    private readonly int _year = year;
    private readonly int _day = day;

    public void Write(IInputProvider inputProvider)
    {
        string path = Input.GetInputFilePath(_path, _year, _day);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string filename = Input.GetInputFileName(path);
        string contents = Input.ConvertToInput(inputProvider.GetInput());

        using StreamWriter streamWriter = File.AppendText(filename);
        streamWriter.Write(contents);
    }
}