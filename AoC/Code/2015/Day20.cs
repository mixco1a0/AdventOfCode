using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2015
{
    class Day20 : Core.Day
    {
        public Day20() { }

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
                    Output = "",
                    RawInput =
@""
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

        private long Part1Sum(long lhn)
        {
            long maxHouse = lhn / 10;
            long[] sums = new long[maxHouse];
            for (long i = 1; i < maxHouse; ++i)
            {
                for (long j = i; j < maxHouse; j += i)
                {
                    sums[j] += i * 10;
                }
            }
            return sums.Select((s, i) => new {val = s, idx = i}).Where(o => o.val >= lhn).Min(o => o.idx);
        }

        private long Part2Sum(long lhn)
        {
            long maxHouse = lhn / 10;
            List<long> sums = [];
            for (int i = 1; i < maxHouse; ++i)
            {
                IEnumerable<int> newValues = Enumerable.Range(1, 50).Reverse().Select(e => e * i);
                foreach (int v in newValues)
                {
                    if (sums.Count < v)
                    {
                        sums.AddRange(Enumerable.Range(sums.Count, v - sums.Count + 1).Select(_ => 0L));
                    }
                    sums[v] += i * 11;
                }
                
                if (sums[i] >= lhn)
                {
                    return i;
                }
            }
            return 0;
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, Func<long, long> sumFunc)
        {
            long lhn = inputs.Select(long.Parse).First();
            return sumFunc(lhn).ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, Part1Sum);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, Part2Sum);
    }
}