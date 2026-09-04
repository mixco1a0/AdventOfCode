using System.IO;
using AoC.Solid.Core.Input;
using AoC.Solid.Core.Interfaces;

namespace AoC.Solid.Core.File;

public class InputFileReader(string path, int year, int day) : IInputReader
{
    private readonly string _path = path;
    private readonly int _year = year;
    private readonly int _day = day;

    public IStringInputProvider Read()
    {
        string path = Utils.File.GetInputFilePath(_path, _year, _day);
        string filename = Utils.File.GetInputFileName(path);

        if (!System.IO.File.Exists(filename))
        {
            throw new FileNotFoundException("File not found.", filename);
        }
        string contents = System.IO.File.ReadAllText(filename);
        return new InlineInputProvider(contents);
    }
}