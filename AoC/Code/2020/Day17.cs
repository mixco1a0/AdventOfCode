using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2020
{
    class Day17 : Day
    {
        public Day17() { }
        public override string GetSolutionVersion(Part part)
        {
            switch (part)
            {
                case Part.One:
                    return "v2";
                case Part.Two:
                    return "v2";
                default:
                    return base.GetSolutionVersion(part);
            }
        }
        protected override List<TestDatum> GetTestData()
        {
            List<TestDatum> testData = new List<TestDatum>();
            testData.Add(new TestDatum
            {
                TestPart = Part.One,
                Output = "112",
                RawInput =
@".#.
..#
###"
            });
            testData.Add(new TestDatum
            {
                TestPart = Part.Two,
                Output = "848",
                RawInput =
@".#.
..#
###"
            });
            return testData;
        }

        private char ProcessCube(Dictionary<string, char> grid, List<int> index)
        {
            int activeCount = Util.ProcessIndexBorder(index, grid, '#');
            string indexKey = Util.GetDynamicIndexKey(index);
            if (!grid.ContainsKey(indexKey))
            {
                grid[indexKey] = '.';
            }
            switch (grid[indexKey])
            {
                case '.':
                    return activeCount == 3 ? '#' : '.';
                case '#':
                    return activeCount >= 2 && activeCount <= 3 ? '#' : '.';
            }
            return '!';
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            Dictionary<string, char> grid = new Dictionary<string, char>();
            for (int y = 0; y < inputs.Count; ++y)
            {
                char[] row = inputs[y].ToCharArray();
                for (int x = 0; x < row.Length; ++x)
                {
                    grid[$"0,{y},{x},"] = row[x];
                }
            }
            List<MinMax> indexRanges = new List<MinMax>();
            indexRanges.Add(new MinMax()); // z [0,0]
            indexRanges.Add(new MinMax(0, inputs.Count - 1)); // y [0,n]
            indexRanges.Add(new MinMax(0, inputs.First().Length - 1)); //x [0,n]
            for (int i = 0; i < 6; ++i)
            {
                foreach (MinMax indexRange in indexRanges)
                {
                    --indexRange.Min;
                    ++indexRange.Max;
                }
                Util.ProcessGrid(ref grid, indexRanges, ProcessCube);
            }
            return grid.Values.Where(c => c == '#').Count().ToString();
        }

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            // TODO: speed up, still a bit slow
            Dictionary<string, char> grid = new Dictionary<string, char>();
            for (int y = 0; y < inputs.Count; ++y)
            {
                char[] row = inputs[y].ToCharArray();
                for (int x = 0; x < row.Length; ++x)
                {
                    grid[$"0,0,{y},{x},"] = row[x];
                }
            }
            List<MinMax> indexRanges = new List<MinMax>();
            indexRanges.Add(new MinMax()); // w [0,0]
            indexRanges.Add(new MinMax()); // z [0,0]
            indexRanges.Add(new MinMax(0, inputs.Count - 1)); // y [0,n]
            indexRanges.Add(new MinMax(0, inputs.First().Length - 1)); //x [0,n]
            for (int i = 0; i < 6; ++i)
            {
                foreach (MinMax indexRange in indexRanges)
                {
                    --indexRange.Min;
                    ++indexRange.Max;
                }
                Util.ProcessGrid(ref grid, indexRanges, ProcessCube);
            }
            return grid.Values.Where(c => c == '#').Count().ToString();
        }
    }
}