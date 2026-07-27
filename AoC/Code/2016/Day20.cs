using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2016
{
    class Day20 : Core.Day
    {
        public Day20() { }

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
                    Output = "3",
                    RawInput =
@"5-8
0-2
4-7
4-5"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Variables = new Dictionary<string, string>() { { nameof(_MinValid), "0" }, { nameof(_MaxValid), "9" } },
                    Output = "1",
                    RawInput =
@"5-8
0-2
5-9
4-5"
                },
            ];
            return testData;
        }
					
#pragma warning disable IDE1006 // Naming Styles
        private static string _MinValid { get; }
        private static string _MaxValid { get; }
#pragma warning restore IDE1006 // Naming Styles

        private record MinMax(long Min, long Max) { }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool findFirst)
        {
            GetVariable(nameof(_MinValid), 0, variables, out long minValid);
            GetVariable(nameof(_MaxValid), (long)uint.MaxValue, variables, out long maxValid);

            List<MinMax> minMax = [.. inputs.Select(i => { long[] split = [.. Util.Number.SplitL(i, '-')]; return new MinMax(split[0], split[1]); })];
            minMax = [.. minMax.OrderByDescending(m => m.Max).OrderBy(m => m.Min)];
            long curMin = minValid;
            long totalAllowed = 0;
            foreach (MinMax mm in minMax)
            {
                if (mm.Max < curMin)
                {
                    continue;
                }

                if (curMin < mm.Min && findFirst)
                {
                    break;
                }
                else if (curMin < mm.Min)
                {
                    totalAllowed += (mm.Min - curMin);
                }

                curMin = mm.Max + 1;
            }
            if (curMin <= maxValid)
            {
                totalAllowed += (maxValid - curMin + 1);
            }

            if (findFirst)
            {
                return curMin.ToString();
            }
            return totalAllowed.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);
    }
}