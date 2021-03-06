using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC
{
    class Util
    {
        static public IEnumerable<string> ConvertInputToList(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new List<string>();
            }
            return input.Split('\n').Select(str => str.Trim('\r'));
        }

        static public void RotateGrid(bool right, ref List<string> grid)
        {
            List<string> newGrid = new List<string>();
            if (right)
            {
                for (int i = 0; i < grid.Count(); ++i)
                {
                    newGrid.Add(string.Join("", grid.Select(r => r.ElementAt(i)).Reverse()));
                }
            }
            else
            {
                int gridCount = grid.Count - 1;
                for (int i = 0; i < grid.Count; ++i)
                {
                    newGrid.Add(string.Join("", grid.Select(r => r.ElementAt(gridCount - i))));
                }
            }
            grid = newGrid;
        }

        static public void FlipGrid(bool horizontal, ref List<string> grid)
        {
            if (horizontal)
            {
                for (int i = 0; i < grid.Count; ++i)
                {
                    grid[i] = string.Join("", grid[i].Reverse());
                }
            }
            else
            {
                grid.Reverse();
            }
        }

        static public void PrintGrid(List<string> grid, Action<string> PrintFunc)
        {
            PrintFunc($"Printing grid {grid.First().Length}x{grid.Count}:");
            foreach (string row in grid)
            {
                PrintFunc(row);
            }
        }
        static public void PrintGrid(List<List<char>> grid, Action<string> PrintFunc)
        {
            PrintFunc($"Printing grid {grid.First().Count}x{grid.Count}:");
            foreach (string row in grid.Select(l => string.Join("", l)))
            {
                PrintFunc(row);
            }
        }

        static public bool ProcessGrid(ref List<List<char>> grid, Func<int, int, List<List<char>>, char> ProcessIndexFunc)
        {
            List<List<char>> newGrid = new List<List<char>>();
            foreach (List<char> row in grid)
            {
                newGrid.Add(new List<char>(row));
            }
            bool complete = true;
            for (int x = 0; x < grid.Count; ++x)
            {
                for (int y = 0; y < grid[x].Count; ++y)
                {
                    newGrid[x][y] = ProcessIndexFunc(x, y, grid);
                    complete = complete && grid[x][y] == newGrid[x][y];
                }
            }
            if (!complete)
            {
                grid = newGrid;
            }
            return !complete;
        }

        static public int ProcessIndexBorder(int x, int y, List<List<char>> grid, char match)
        {
            int borderMatch = 0;
            for (int i = x - 1; i <= x + 1; ++i)
            {
                if (i < 0 || i >= grid.Count)
                {
                    continue;
                }

                for (int j = y - 1; j <= y + 1; ++j)
                {
                    if (i == x && j == y)
                    {
                        continue;
                    }

                    if (j < 0 || j >= grid[x].Count)
                    {
                        continue;
                    }

                    if (grid[i][j] == match)
                    {
                        ++borderMatch;
                    }
                }
            }
            return borderMatch;
        }

        static public Dictionary<char, int> ProcessIndexBorder(int x, int y, List<List<char>> grid)
        {
            Dictionary<char, int> borderValues = new Dictionary<char, int>();
            for (int i = x - 1; i <= x + 1; ++i)
            {
                if (i < 0 || i >= grid.Count)
                {
                    continue;
                }

                for (int j = y - 1; j <= y + 1; ++j)
                {
                    if (i == x && j == y)
                    {
                        continue;
                    }

                    if (j < 0 || j >= grid[x].Count)
                    {
                        continue;
                    }

                    char borderValue = grid[i][j];
                    if (!borderValues.ContainsKey(borderValue))
                    {
                        borderValues[borderValue] = 1;
                    }
                    else
                    {
                        ++borderValues[borderValue];
                    }
                }
            }
            return borderValues;
        }

        static public string GetDynamicIndexKey(List<int> index)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int i in index)
            {
                sb.Append(i);
                sb.Append(',');
            }
            return sb.ToString();
        }

        static public bool ProcessGrid(ref Dictionary<string, char> grid, List<MinMax> indexRanges, Func<Dictionary<string, char>, List<int>, char> ProcessIndexFunc)
        {
            bool complete = true;

            List<int> index = indexRanges.Select(r => r.Min).ToList();
            Dictionary<string, char> newGrid = new Dictionary<string, char>();
            while (true)
            {
                for (int i = 0; i < indexRanges.Count && index[i] > indexRanges[i].Max;)
                {
                    index[i] = indexRanges[i].Min;
                    if (++i < indexRanges.Count)
                    {
                        ++index[i];
                    }
                    else
                    {
                        grid = newGrid;
                        return !complete;
                    }
                }

                char newVal = ProcessIndexFunc(grid, index);
                string indexKey = GetDynamicIndexKey(index);
                if (grid.ContainsKey(indexKey))
                {
                    complete = complete && newVal != grid[indexKey];
                }
                else
                {
                    complete = false;
                }
                newGrid[indexKey] = newVal;

                ++index[0];
            }
        }


        static public int ProcessIndexBorder(List<int> index, Dictionary<string, char> grid, char match)
        {
            int borderMatch = 0;
            string indexKey = GetDynamicIndexKey(index);
            List<MinMax> indexRanges = index.Select(i => new MinMax(i - 1, i + 1)).ToList();
            List<int> borderIndex = indexRanges.Select(r => r.Min).ToList();
            while (true)
            {
                for (int i = 0; i < indexRanges.Count && borderIndex[i] > indexRanges[i].Max;)
                {
                    borderIndex[i] = indexRanges[i].Min;
                    if (++i < indexRanges.Count)
                    {
                        ++borderIndex[i];
                    }
                    else
                    {
                        return borderMatch;
                    }
                }

                string borderIndexKey = GetDynamicIndexKey(borderIndex);
                if (borderIndexKey != indexKey && grid.ContainsKey(borderIndexKey) && grid[borderIndexKey] == match)
                {
                    ++borderMatch;
                }

                ++borderIndex[0];
            }
        }

        static public string GetLogTimeStamp()
        {
            return $"|{DateTime.Now.ToString("hh:mm:ss.fff")}| ";
        }

        private static string s_workingDirectory;
        static public string WorkingDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(s_workingDirectory))
                {
                    string curDir = Directory.GetCurrentDirectory();
                    string dirRoot = Path.GetPathRoot(curDir);
                    while (true)
                    {
                        if (curDir == dirRoot)
                        {
                            break;
                        }

                        if (Path.GetFileName(curDir) == nameof(AoC))
                        {
                            break;
                        }

                        curDir = Path.GetDirectoryName(curDir);
                    }

                    if (curDir != dirRoot)
                    {
                        s_workingDirectory = curDir;
                    }
                    else
                    {
                        throw new DirectoryNotFoundException($"Unable to find base directory */{nameof(AoC)}/*");
                    }
                }
                return s_workingDirectory;
            }
        }

    }

    public class MinMax
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public MinMax()
        {
            Min = 0;
            Max = 0;
        }
        public MinMax(int min, int max)
        {
            Min = min;
            Max = max;
        }
        public bool GTE_LTE(int val) { return Min <= val && val <= Max; }
        public bool GT_LTE(int val) { return Min < val && val <= Max; }
        public bool GTE_LT(int val) { return Min <= val && val < Max; }
        public bool GT_LT(int val) { return Min < val && val < Max; }
    }
}