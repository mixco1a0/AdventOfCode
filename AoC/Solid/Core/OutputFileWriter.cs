using System.IO;
using AoC.Solid.Core.Interfaces;

namespace AoC.Solid.Core;

public class OutputFileWriter(string path, int year, int day, int part) : IOutputWriter
{
    private readonly string _path = path;
    private readonly int _year = year;
    private readonly int _day = day;
    private readonly int _part = part;

    public void Write(string output)
    {
        string path = Utils.File.GetOutputFilePath(_path, _year, _day);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string filename = Utils.File.GetOutputFileName(path, _part);
        using StreamWriter streamWriter = File.CreateText(filename);
        streamWriter.Write(output);
    }
}