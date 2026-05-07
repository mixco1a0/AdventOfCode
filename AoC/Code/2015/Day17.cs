using System.Collections.Generic;
using System.Linq;

namespace AoC._2015
{
    class Day17 : Core.Day
    {
        public Day17() { }

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
                    Variables = new Dictionary<string, string> { { nameof(_Liters), "25" } },
                    Output = "4",
                    RawInput =
@"20
15
10
5
5"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Variables = new Dictionary<string, string> { { nameof(_Liters), "25" } },
                    Output = "3",
                    RawInput =
@"20
15
10
5
5"
                },
            ];
            return testData;
        }

#pragma warning disable IDE1006 // Naming Styles
        private static int _Liters { get; }
#pragma warning restore IDE1006 // Naming Styles

        private static int GetTotal(IEnumerable<int> inputs)
        {
            int total = 0;
            foreach (int input in inputs)
            {
                total += input;
            }
            return total;
        }

        private static int TryNext(int result, List<int> inputs, out int uniqueMin)
        {
            uniqueMin = 0;
            int total = 0;
            int count = 0;
            List<bool> bools = [.. inputs.Select(_ => false)];
            Dictionary<int, int> solutionCount = [];
            if (inputs.Count > 0)
            {
                for (int i = 0; i < inputs.Count; ++i)
                {
                    // impossible to reach the total using remaining numbers, skip ahead
                    if (total + GetTotal(inputs.Skip(i)) < result)
                    {
                        i = inputs.Count - 1;
                    }

                    total += inputs[i];
                    if (total > result)
                    {
                        total -= inputs[i];
                    }
                    else if (total == result)
                    {
                        bools[i] = true;
                        int used = bools.Count(b => b);
                        if (!solutionCount.TryGetValue(used, out int value))
                        {
                            solutionCount[used] = 1;
                        }
                        else
                        {
                            solutionCount[used] = ++value;
                        }
                        // Log(Core.Log.ELevel.Spam, $"VALID: {string.Join(',', bools.Select((b, i) => new { b = b, i = i }).Where(pair => pair.b).Select(pair => $"{inputs[pair.i]}[#{pair.i}]"))}");
                        bools[i] = false;
                        total -= inputs[i];
                        ++count;
                    }
                    else
                    {
                        bools[i] = true;
                    }

                    if (i + 1 == inputs.Count)
                    {
                        bools[i] = false;
                        while (i >= 0 && !bools[i])
                        {
                            --i;
                        }
                        if (i < 0)
                        {
                            int minKey = solutionCount.Keys.Min();
                            uniqueMin = solutionCount[minKey];
                            return count;
                        }
                        bools[i] = false;

                        IEnumerable<int> used = inputs.Select((num, idx) => (num, idx)).Where(pair => bools[pair.idx]).Select(pair => pair.num);
                        total = 0;
                        foreach (int num in used)
                        {
                            total += num;
                        }
                    }
                }
            }
            return count;
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool findUnique)
        {
            GetVariable(nameof(_Liters), 150, variables, out int liters);

            int nextVal = TryNext(liters, [.. inputs.Select(int.Parse).OrderByDescending(_ => _)], out int uniqueMin);
            if (findUnique)
            {
                return uniqueMin.ToString();
            }
            return nextVal.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}