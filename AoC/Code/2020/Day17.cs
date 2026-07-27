using System.Collections.Generic;
using System.Linq;

namespace AoC._2020
{
    class Day17 : Core.Day
    {
        public Day17() { }

        public override string GetSolutionVersion(Core.Part part)
        {
            return part switch
            {
                Core.Part.One => "v2",
                Core.Part.Two => "v2",
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
                    Output = "112",
                    RawInput =
@".#.
..#
###"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "848",
                    RawInput =
@".#.
..#
###"
                },
            ];
            return testData;
        }

        private char ProcessCube(Dictionary<string, char> grid, List<int> index)
        {
            int activeCount = Util.Grid2.ProcessIndexBorder(index, grid, '#');
            string indexKey = Util.Grid2.GetDynamicIndexKey(index);
            if (!grid.ContainsKey(indexKey))
            {
                grid[indexKey] = '.';
            }
            return grid[indexKey] switch
            {
                '.' => activeCount == 3 ? '#' : '.',
                '#' => activeCount >= 2 && activeCount <= 3 ? '#' : '.',
                _ => '!',
            };
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            Dictionary<string, char> grid = [];
            for (int y = 0; y < inputs.Count; ++y)
            {
                char[] row = inputs[y].ToCharArray();
                for (int x = 0; x < row.Length; ++x)
                {
                    grid[$"0,{y},{x},"] = row[x];
                }
            }
            List<Base.Range> indexRanges =
            [
                new Base.Range(), // z [0,0]
                new Base.Range(0, inputs.Count - 1), // y [0,n]
                new Base.Range(0, inputs.First().Length - 1), //x [0,n]
            ];
            for (int i = 0; i < 6; ++i)
            {
                foreach (Base.Range indexRange in indexRanges)
                {
                    --indexRange.Min;
                    ++indexRange.Max;
                }
                Util.Grid2.Process(ref grid, indexRanges, ProcessCube);
            }
            return grid.Values.Where(c => c == '#').Count().ToString();
        }

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            // TODO: speed up, still a bit slow
            Dictionary<string, char> grid = [];
            for (int y = 0; y < inputs.Count; ++y)
            {
                char[] row = inputs[y].ToCharArray();
                for (int x = 0; x < row.Length; ++x)
                {
                    grid[$"0,0,{y},{x},"] = row[x];
                }
            }
            List<Base.Range> indexRanges =
            [
                new Base.Range(), // w [0,0]
                new Base.Range(), // z [0,0]
                new Base.Range(0, inputs.Count - 1), // y [0,n]
                new Base.Range(0, inputs.First().Length - 1), //x [0,n]
            ];
            for (int i = 0; i < 6; ++i)
            {
                foreach (Base.Range indexRange in indexRanges)
                {
                    --indexRange.Min;
                    ++indexRange.Max;
                }
                Util.Grid2.Process(ref grid, indexRanges, ProcessCube);
            }
            return grid.Values.Where(c => c == '#').Count().ToString();
        }
    }
}