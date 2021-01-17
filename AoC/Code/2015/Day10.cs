using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2015
{
    class Day10 : Day
    {
        private static int sTimes = 40;
        public Day10() { }
        public override string GetSolutionVersion(Part part)
        {
            switch (part)
            {
                // case Part.One:
                //     return "v1";
                // case Part.Two:
                //     return "v1";
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
                Variables = new Dictionary<string, string> { { nameof(sTimes), "5" } },
                Output = "6",
                RawInput =
@"1"
            });
            return testData;
        }

        private string Process(string input)
        {
            string processed = "";
            char cur = input.First();
            int count = 1;
            foreach (char c in input[1..])
            {
                if (c == cur)
                {
                    ++count;
                }
                else
                {
                    processed += $"{count}{cur}";
                    cur = c;
                    count = 1;
                }
            }
            processed += $"{count}{cur}";
            // DebugWriteLine($"Before:{input} - After:{processed}");
            return processed;
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            int times = 40;
            if (variables != null && variables.ContainsKey(nameof(sTimes)))
            {
                times = int.Parse(variables[nameof(sTimes)]);
            }
            string input = inputs.First();
            for (int i = 0; i < times; ++i)
            {
                input = Process(input);
                DebugWriteLine($"{i} complete [{input.Length}]");
            }
            return input.Length.ToString();
        }

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            int times = 50;
            if (variables != null && variables.ContainsKey(nameof(sTimes)))
            {
                times = int.Parse(variables[nameof(sTimes)]);
            }
            string input = inputs.First();
            for (int i = 0; i < times; ++i)
            {
                input = Process(input);
                DebugWriteLine($"{i} complete [{input.Length}]");
            }
            return input.Length.ToString();
        }
    }
}