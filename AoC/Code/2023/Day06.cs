using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2023
{
    class Day06 : Core.Day
    {
        public Day06() { }

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
                    Output = "288",
                    RawInput =
@"Time:      7  15   30
Distance:  9  40  200"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "71503",
                    RawInput =
@"Time:      7  15   30
Distance:  9  40  200"
                },
            ];
            return testData;
        }

        public long GetSolutionCount(long time, long distance)
        {
            long solutions = 0;
            for (long i = 1; i < time; ++i)
            {
                long remainingTime = time - i;
                if (remainingTime * i > distance)
                {
                    ++solutions;
                }
            }
            return solutions;
        }

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool oneRace)
        {
            List<long> times;
            List<long> distances;
            if (oneRace)
            {
                times =
                [

                    long.Parse(string.Join("", Util.Number.SplitL(inputs[0], ' ')))
                ];
                distances =
                [
                    long.Parse(string.Join("", Util.Number.SplitL(inputs[1], ' ')))
                ];
            }
            else
            {
                times = [.. Util.Number.SplitL(inputs[0], ' ')];
                distances = [.. Util.Number.SplitL(inputs[1], ' ')];
            }
            long answer = 1;
            for (int i = 0; i < times.Count; ++i)
            {
                answer *= GetSolutionCount(times[i], distances[i]);
            }
            return answer.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}