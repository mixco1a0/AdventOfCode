using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2016
{
    class Day03 : Core.Day
    {
        public Day03() { }

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
                    Output = "1",
                    RawInput =
@"5 10 25
3 4 5
5 10 30"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "",
                    RawInput =
@""
                },
            ];
            return testData;
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            int possible = 0;
            foreach (string input in inputs)
            {
                List<int> sides = [.. Util.Number.Split(input, ' ')];
                sides.Sort();
                if (sides[0] + sides[1] > sides[2])
                {
                    possible++;
                }
            }
            return possible.ToString();
        }

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            int possible = 0;
            for (int i = 0; i + 2 < inputs.Count;)
            {
                List<int> sidesA = [.. Util.Number.Split(inputs[i], ' ')];
                List<int> sidesB = [.. Util.Number.Split(inputs[i + 1], ' ')];
                List<int> sidesC = [.. Util.Number.Split(inputs[i + 2], ' ')];
                for (int j = 0; j < 3; ++j, ++i)
                {
                    List<int> cur = [.. sidesA.Skip(j).Take(1), .. sidesB.Skip(j).Take(1), .. sidesC.Skip(j).Take(1)];
                    cur.Sort();
                    if (cur[0] + cur[1] > cur[2])
                    {
                        possible++;
                    }
                }
            }
            return possible.ToString();
        }
    }
}