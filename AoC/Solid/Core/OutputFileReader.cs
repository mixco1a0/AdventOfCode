using System.IO;
using AoC.Solid.Core.Interfaces;

namespace AoC.Solid.Core;


public class OutputFileReader(string path, int year, int day, int part) : IOutputReader
{
    private readonly string _path = path;
    private readonly int _year = year;
    private readonly int _day = day;
    private readonly int _part = part;

    public string Read()
    {
        string path = Utils.File.GetOutputFilePath(_path, _year, _day);
        string filename = Utils.File.GetOutputFileName(path, _part);
        
        if (!File.Exists(filename))
        {
            throw new FileNotFoundException("File not found.", filename);
        }
        return File.ReadAllText(filename).Trim();
    }
}