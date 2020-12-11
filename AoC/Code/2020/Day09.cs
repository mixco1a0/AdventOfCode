using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2020
{
    class Day09 : Day
    {
        public Day09() { }

        public override string GetSolutionVersion(TestPart testPart)
        {
            switch (testPart)
            {
                case TestPart.One:
                    return "1";
                case TestPart.Two:
                    return "1";
                default:
                    return base.GetSolutionVersion(testPart);
            }
        }

        protected override List<TestDatum> GetTestData()
        {
            List<TestDatum> testData = new List<TestDatum>();
            testData.Add(new TestDatum
            {
                TestPart = TestPart.One,
                Variables = new Dictionary<string, string> { { "preamble", "5" } },
                Output = "127",
                RawInput =
@"35
20
15
25
47
40
62
55
65
95
102
117
150
182
127
219
299
277
309
576"
            });
            testData.Add(new TestDatum
            {
                TestPart = TestPart.Two,
                Variables = new Dictionary<string, string> { { "preamble", "5" } },
                Output = "62",
                RawInput =
@"35
20
15
25
47
40
62
55
65
95
102
117
150
182
127
219
299
277
309
576"
            });
            return testData;
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            int preamble = 25;
            if (variables != null && variables.ContainsKey(nameof(preamble)))
            {
                preamble = int.Parse(variables[nameof(preamble)]);
            }
            return GetWeakness(inputs, preamble);
        }

        private string GetWeakness(List<string> inputs, int preamble)
        {
            List<long> numbers = inputs.Select(num => long.Parse(num)).ToList();
            for (int i = preamble; i < numbers.Count(); ++i)
            {
                if (!FindSum(numbers.Skip(i - preamble).Take(preamble).ToList(), numbers[i]))
                    return numbers[i].ToString();
            }
            return "NaN";
        }

        private bool FindSum(List<long> list, long num)
        {
            for (int i = 0; i < list.Count(); ++i)
            {
                for (int j = i + 1; j < list.Count(); ++j)
                {
                    if (list[i] + list[j] == num)
                        return true;
                }
            }

            return false;
        }

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            int preamble = 25;
            if (variables != null && variables.ContainsKey(nameof(preamble)))
            {
                preamble = int.Parse(variables[nameof(preamble)]);
            }

            long weakness = long.Parse(GetWeakness(inputs, preamble));

            List<long> numbers = inputs.Select(num => long.Parse(num)).ToList();
            for (int i = 0; i < numbers.Count(); ++i)
            {
                long runningTotal = numbers[i];
                for (int j = i + 1; j < numbers.Count(); ++j)
                {
                    runningTotal += numbers[j];
                    if (runningTotal == weakness)
                    {
                        List<long> subset = numbers.Skip(i).Take(j - i + 1).ToList();
                        subset.Sort();
                        return (subset.First() + subset.Last()).ToString();
                    }
                    else if (runningTotal > weakness)
                        break;
                }
            }

            return "NaN";
        }
    }
}