using System.IO;
using AoC.Solid.Core.Interfaces;

namespace AoC.Solid.Core;


public class InputFileReader(string path, int year, int day) : IReader
{
    private readonly string _path = path;
    private readonly int _year = year;
    private readonly int _day = day;

    public string Read()
    {
        string path = Utils.Puzzle.GetInputFilePath(_path, _year, _day);
        string filename = Utils.Puzzle.GetInputFileName(path);
        
        if (!File.Exists(filename))
        {
            throw new FileNotFoundException("File not found.", filename);
        }
        return File.ReadAllText(filename);
    }
}