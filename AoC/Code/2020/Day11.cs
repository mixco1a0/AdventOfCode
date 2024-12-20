using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2020
{
    class Day11 : Core.Day
    {
        public Day11() { }

        public override string GetSolutionVersion(Core.Part part)
        {
            switch (part)
            {
                case Core.Part.One:
                    return "v2";
                case Core.Part.Two:
                    return "v1";
                default:
                    return base.GetSolutionVersion(part);
            }
        }

        protected override List<Core.TestDatum> GetTestData()
        {
            List<Core.TestDatum> testData = new List<Core.TestDatum>();
            testData.Add(new Core.TestDatum
            {
                TestPart = Core.Part.One,
                Output = "37",
                RawInput =
@"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL"
            });
            testData.Add(new Core.TestDatum
            {
                TestPart = Core.Part.Two,
                Output = "26",
                RawInput =
@"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL"
            });
            return testData;
        }

        private char GetLocationState(int x, int y, List<List<char>> seats)
        {
            if (seats[x][y] == '.')
            {
                return '.';
            }

            int fullCount = Util.Grid.ProcessIndexBorder(x, y, seats, '#');

            switch (seats[x][y])
            {
                case 'L':
                    return fullCount > 0 ? 'L' : '#';
                case '#':
                    return fullCount >= 4 ? 'L' : '#';
            }

            return '!';
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            List<List<char>> seats = inputs.Select(a => a.ToCharArray().ToList()).ToList();
            while (Util.Grid.ProcessGrid(ref seats, GetLocationState)) ;
            return string.Join("", seats.Select(c => string.Join("", c))).Replace(".", "").Replace("L", "").Count().ToString();
        }


        private char GetLocationStateStringent(int x, int y, List<List<char>> seats)
        {
            if (seats[x][y] == '.')
            {
                return '.';
            }

            int fullCount = 0;
            HashSet<string> checkedLocations = new HashSet<string>();
            for (int i = x - 1; i <= x + 1; ++i)
            {
                if (i < 0 || i >= seats.Count)
                {
                    continue;
                }

                for (int j = y - 1; j <= y + 1; ++j)
                {
                    if (i == x && j == y)
                    {
                        continue;
                    }

                    if (j < 0 || j >= seats[x].Count)
                    {
                        continue;
                    }

                    if (seats[i][j] == '#')
                    {
                        checkedLocations.Add($"{i}.{j}");
                        ++fullCount;
                    }

                    if (seats[i][j] == 'L')
                    {
                        checkedLocations.Add($"{i}.{j}");
                    }
                }
            }

            Func<int, int> Add = idx => { return idx + 1; };
            Func<int, int> Sub = idx => { return idx - 1; };
            Func<int, int> Ign = idx => { return idx; };

            fullCount += GetExtendedViewCount(x, y, seats, checkedLocations, Sub, Sub);
            fullCount += GetExtendedViewCount(x, y, seats, checkedLocations, Sub, Ign);
            fullCount += GetExtendedViewCount(x, y, seats, checkedLocations, Sub, Add);
            fullCount += GetExtendedViewCount(x, y, seats, checkedLocations, Ign, Sub);
            fullCount += GetExtendedViewCount(x, y, seats, checkedLocations, Ign, Add);
            fullCount += GetExtendedViewCount(x, y, seats, checkedLocations, Add, Sub);
            fullCount += GetExtendedViewCount(x, y, seats, checkedLocations, Add, Ign);
            fullCount += GetExtendedViewCount(x, y, seats, checkedLocations, Add, Add);

            switch (seats[x][y])
            {
                case 'L':
                    return fullCount > 0 ? 'L' : '#';
                case '#':
                    return fullCount >= 5 ? 'L' : '#';
            }


            return '!';
        }

        private int GetExtendedViewCount(int x, int y, List<List<char>> seats, HashSet<string> checkedLocations, Func<int, int> xFunc, Func<int, int> yFunc)
        {
            int fullCount = 0;
            for (int i = xFunc(x), j = yFunc(y); ; i = xFunc(i), j = yFunc(j))
            {
                if (i == x && j == y)
                {
                    continue;
                }

                if (i < 0 || j < 0 || i >= seats.Count || j >= seats[i].Count)
                {
                    break;
                }

                if (seats[i][j] == '#')
                {
                    if (!checkedLocations.Contains($"{i}.{j}"))
                    {
                        ++fullCount;
                    }
                    break;
                }

                if (seats[i][j] == 'L')
                {
                    break;
                }
            }
            return fullCount;
        }

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            List<List<char>> seats = inputs.Select(a => a.ToCharArray().ToList()).ToList();
            while (Util.Grid.ProcessGrid(ref seats, GetLocationStateStringent)) ;
            return string.Join("", seats.Select(c => string.Join("", c))).Replace(".", "").Replace("L", "").Count().ToString();
        }
    }
}