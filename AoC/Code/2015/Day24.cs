using System.Collections.Generic;
using System.Linq;

namespace AoC._2015
{
    class Day24 : Core.Day
    {
        public Day24() { }

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
                    Output = "99",
                    RawInput =
@"1
2
3
4
5
7
8
9
10
11"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "44",
                    RawInput =
@"1
2
3
4
5
7
8
9
10
11"
                },
            ];
            return testData;
        }

        record QuantumEntanglement(long Value, int Count, List<int> Weights) { }
        static readonly QuantumEntanglement MaxQE = new(int.MaxValue, int.MaxValue, []);

        private static QuantumEntanglement OptimizedEQ(QuantumEntanglement a, QuantumEntanglement b)
        {
            if (a.Count < b.Count)
            {
                return a;
            }

            if (b.Count < a.Count)
            {
                return b;
            }

            if (a.Value < b.Value)
            {
                return a;
            }

            return b;
        }

        private static QuantumEntanglement GenerateQuantumEntanglement(List<int> weights)
        {
            long mult = 1;
            weights.ForEach(i => mult *= i);
            return new QuantumEntanglement(mult, weights.Count, weights);
        }

        private static void Find(int maxSum, List<int> pending, List<int> a, bool checkMaxQE, ref QuantumEntanglement best)
        {
            // check for possible match
            int sumA = 0;
            a.ForEach(i => sumA += i);
            if (sumA == maxSum)
            {
                long val = best.Value;
                best = OptimizedEQ(best, GenerateQuantumEntanglement(a));
                return;
            }

            // ignore anything worse than current
            if (best.Weights.Count > 0 && a.Count >= best.Weights.Count)
            {
                return;
            }

            // check next weight
            foreach (int p in pending)
            {
                if (sumA + p <= maxSum)
                {
                    List<int> newList = [.. a];
                    newList.Add(p);
                    Find(maxSum, [.. pending.Where(i => i != p)], newList, checkMaxQE, ref best);
                }

                if (checkMaxQE && best != MaxQE)
                {
                    return;
                }
            }
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, int groupSize, bool checkMaxQE)
        {
            int maxSum = 0;
            List<int> numbers = [.. inputs.Select(int.Parse)];
            numbers.ForEach(n => maxSum += n);
            maxSum /= groupSize;
            numbers.Sort();
            numbers.Reverse();
            QuantumEntanglement best = MaxQE;
            Find(maxSum, numbers, [], checkMaxQE, ref best);
            return best.Value.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, 3, true);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, 4, false);
    }
}