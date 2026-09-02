using System.IO;
using AoC.Solid.Core.Interfaces;

namespace AoC.Solid.Core;

public class InputFileWriter(string path, int year, int day) : IInputWriter
{
    private readonly string _path = path;
    private readonly int _year = year;
    private readonly int _day = day;

    public void Write(IStringInputProvider inputProvider)
    {
        string path = Utils.File.GetInputFilePath(_path, _year, _day);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string filename = Utils.File.GetInputFileName(path);
        string contents = Utils.Input.ConvertToInput(inputProvider.GetInput());

        using StreamWriter streamWriter = File.AppendText(filename);
        streamWriter.Write(contents);
    }
}