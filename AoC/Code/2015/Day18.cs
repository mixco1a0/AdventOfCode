using System.Collections.Generic;
using System.Linq;

namespace AoC._2015
{
    class Day18 : Core.Day
    {
        public Day18() { }

        public override string GetSolutionVersion(Core.Part part)
        {
            return part switch
            {
                Core.Part.One => "v1",
                Core.Part.Two => "v1",
                _ => base.GetSolutionVersion(part),
            };
        }

        protected override List<Core.TestDatum> GetTestData()
        {
            List<Core.TestDatum> testData =
            [
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Variables = new Dictionary<string, string> { { nameof(_Steps), "4" } },
                    Output = "4",
                    RawInput =
@".#.#.#
...##.
#....#
..#...
#.#..#
####.."
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Variables = new Dictionary<string, string> { { nameof(_Steps), "5" } },
                    Output = "17",
                    RawInput =
@"##.#.#
...##.
#....#
..#...
#.#..#
####.#"
                },
            ];
            return testData;
        }

#pragma warning disable IDE1006 // Naming Styles
        private int _Steps { get; }
#pragma warning restore IDE1006 // Naming Styles

        private static char GetLocationState(int x, int y, List<List<char>> lights)
        {
            int onCount = Util.Grid2.ProcessIndexBorder(x, y, lights, '#');

            return lights[x][y] switch
            {
                '.' => onCount == 3 ? '#' : '.',
                '#' => onCount == 2 || onCount == 3 ? '#' : '.',
                _ => '!',
            };
        }

        private static char GetLocationStateCornersOn(int x, int y, List<List<char>> lights)
        {
            if (x == 0 && (y == 0 || y == lights.Count - 1))
            {
                return '#';
            }
            else if (y == 0 && (x == 0 || x == lights.First().Count - 1))
            {
                return '#';
            }
            else if (x == lights.First().Count - 1 && y == lights.Count - 1)
            {
                return '#';
            }

            int onCount = Util.Grid2.ProcessIndexBorder(x, y, lights, '#');

            return lights[x][y] switch
            {
                '.' => onCount == 3 ? '#' : '.',
                '#' => onCount == 2 || onCount == 3 ? '#' : '.',
                _ => '!',
            };
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool checkCorners)
        {
            GetVariable(nameof(_Steps), 100, variables, out int steps);

            List<List<char>> lights = [.. inputs.Select(a => a.ToCharArray().ToList())];

            if (checkCorners)
            {
                lights[0][0] = '#';
                lights[0][^1] = '#';
                lights[^1][0] = '#';
                lights[^1][lights[0].Count - 1] = '#';
            }

            // Util.Grid.Print2D(Core.Log.ELevel.Spam, lights);
            for (int i = 0; i < steps; ++i)
            {
                if (checkCorners)
                {
                    Util.Grid2.Process(ref lights, GetLocationStateCornersOn);
                    // Core.Log.WriteLine(Core.Log.ELevel.Spam, $"After {i + 1} step");
                    // Util.Grid.Print2D(Core.Log.ELevel.Spam, lights);
                }
                else
                {
                    Util.Grid2.Process(ref lights, GetLocationState);
                }
            }
            return string.Join("", lights.Select(c => string.Join("", c))).Replace(".", "").Length.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}