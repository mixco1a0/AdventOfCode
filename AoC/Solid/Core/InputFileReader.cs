using System.IO;
using AoC.Solid.Core.Interfaces;
using AoC.Solid.Utils;

namespace AoC.Solid.Core;


public class InputFileReader(string path, int year, int day) : IReader
{
    private readonly string _path = path;
    private readonly int _year = year;
    private readonly int _day = day;

    public IInputProvider Read()
    {
        string path = Input.GetInputFilePath(_path, _year, _day);
        string filename = Input.GetInputFileName(path);
        
        if (!File.Exists(filename))
        {
            throw new FileNotFoundException("File not found.", filename);
        }
        string contents = File.ReadAllText(filename);
        return new InlineInputProvider(contents);
    }
}