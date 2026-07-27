using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC._2016
{
    class Day16 : Core.Day
    {
        public Day16() { }

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
                    Variables = new Dictionary<string, string>() {{nameof(_DiskLength), "20"}},
                    Output = "01100",
                    RawInput =
@"10000"
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

#pragma warning disable IDE1006 // Naming Styles
        private static string _DiskLength { get; }
#pragma warning restore IDE1006 // Naming Styles

        private static string FillDisk(string a)
        {
            string b = string.Join("", a.Replace('0', '#').Replace('1', '0').Replace('#', '1').Reverse());
            StringBuilder sb = new(a);
            sb.Append('0');
            return sb.Append(b).ToString();
        }

        private static string GetChecksum(string disk)
        {
            StringBuilder sb = new();
            for (int i = 0; i + 1 < disk.Length; i += 2)
            {
                sb.Append(disk[i] == disk[i+1] ? '1' : '0');
            }
            return sb.ToString();
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, int diskLength)
        {
            GetVariable(nameof(_DiskLength), diskLength, variables, out diskLength);

            string disk = inputs.First();
            while (disk.Length < diskLength)
            {
                disk = FillDisk(disk);
            }

            string checkSum = disk.Substring(0, diskLength);
            while (checkSum.Length % 2 == 0)
            {
                checkSum = GetChecksum(checkSum);
            }

            return checkSum;
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, 272);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, 35651584);
    }
}