using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AoC._2015
{
    class Day04 : Core.Day
    {
        public Day04() { }

        public override string GetSolutionVersion(Core.Part part)
        {
            return part switch
            {
                Core.Part.One => "v3",
                Core.Part.Two => "v3",
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
                    Output = "609043",
                    RawInput =
@"abcdef"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Output = "1048970",
                    RawInput =
@"pqrstuv"
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

        private static bool HasLeadingZeroes(byte[] hashBytes, int leadingZeroes)
        {
            for (int z = 0; z < leadingZeroes; ++z)
            {
                byte mask = (z % 2 == 0) ? (byte)0xf0 : (byte)0x0f;
                if ((hashBytes[z / 2] & mask) != 0x00)
                {
                    return false;
                }
            }
            return true;
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, int leadingZeroes)
        {
            string input = inputs[0];
            for (int i = 0; i < int.MaxValue; ++i)
            {
                StringBuilder sb = new(input);
                sb.Append(i);
                byte[] inputBytes = Encoding.ASCII.GetBytes(sb.ToString());
                byte[] hashBytes = MD5.HashData(inputBytes);
                // byte[] hashBytes = null;
                // Action compute = () =>
                // {
                //     hashBytes = MD5.HashData(inputBytes);
                // };
                // WasteTime(compute);
                if (HasLeadingZeroes(hashBytes, leadingZeroes))
                {
                    return i.ToString();
                }
            }

            return string.Empty;
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, 5);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, 6);
    }
}