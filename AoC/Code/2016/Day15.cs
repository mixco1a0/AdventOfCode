using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2016
{
    class Day15 : Core.Day
    {
        public Day15() { }

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
                    Output = "5",
                    RawInput =
@"Disc #1 has 5 positions; at time=0, it is at position 4.
Disc #2 has 2 positions; at time=0, it is at position 1."
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

        private record Disk(int Id, int InitialPos, int MaxPositions)
        {
            static public Disk Parse(string input)
            {
                int[] values = [.. Util.Number.Split(input, " #=,.")];
                return new Disk(values[0], values[3], values[1]);
            }
            public int GetPosition(int time)
            {
                return (Id + InitialPos + time) % MaxPositions;
            }
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, Disk extraDisk)
        {
            Disk[] disks = [.. inputs.Select(Disk.Parse)];
            if (extraDisk != null)
            {
                disks = [.. disks, extraDisk];
            }

            int time = 0;
            while (true)
            {
                bool fellThrough = true;
                for (int d = 0; fellThrough && d < disks.Length; ++d)
                {
                    fellThrough = (disks[d].GetPosition(time) == 0);
                }
                if (fellThrough)
                {
                    break;
                }
                ++time;
            }
            return time.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, null);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, new Disk(inputs.Count + 1, 0, 11));
    }
}