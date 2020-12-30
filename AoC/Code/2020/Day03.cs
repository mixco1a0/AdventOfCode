using System.Collections.Generic;
using System.Linq;

namespace AoC._2020
{
    class Day03 : Day
    {
        public Day03() { }
        public override string GetSolutionVersion(Part part)
        {
            switch (part)
            {
                case Part.One:
                    return "v1";
                case Part.Two:
                    return "v1";
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
                Output = "7",
                RawInput =
@"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#"
            });
            testData.Add(new TestDatum
            {
                TestPart = Part.Two,
                Output = "336",
                RawInput =
@"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#"
            });
            return testData;
        }

        private long SlopeCheck(int rightStep, int downStep, List<string> inputs)
        {
            int curIdx = 0;
            long treeCount = 0;
            for (int i = downStep; i < inputs.Count; i += downStep)
            {
                curIdx = (curIdx + rightStep) % inputs[i].Length;
                if (inputs[i].ElementAt(curIdx) == '#')
                {
                    ++treeCount;
                }
            }
            return treeCount;
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            return SlopeCheck(3, 1, inputs).ToString();
        }

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            /*
            Right 1, down 1.
            Right 3, down 1. (This is the slope you already checked.)
            Right 5, down 1.
            Right 7, down 1.
            Right 1, down 2.
            */

            return (SlopeCheck(1, 1, inputs) *
                    SlopeCheck(3, 1, inputs) *
                    SlopeCheck(5, 1, inputs) *
                    SlopeCheck(7, 1, inputs) *
                    SlopeCheck(1, 2, inputs)).ToString();
        }
    }
}