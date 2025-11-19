using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2024
{
    class Day22 : Core.Day
    {
        public Day22() { }

        public override string GetSolutionVersion(Core.Part part)
        {
            return part switch
            {
                Core.Part.One => "v1",
                // Core.Part.Two => "v1",
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
                    Output = "5908254",
                    Variables = new Dictionary<string, string> { { nameof(_SecretNumberCount), "10" } },
                    RawInput =
@"123"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Output = "37327623",
                    RawInput =
@"1
10
100
2024"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "1",
                    RawInput =
@"123"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "23",
                    RawInput =
@"1
2
3
2024"
                },
            ];
            return testData;
        }

#pragma warning disable IDE1006 // Naming Styles
        private static int _SecretNumberCount { get; }
#pragma warning restore IDE1006 // Naming Styles

        private class Secret
        {
            public long Number { get; set; }
            public byte PriceChange { get; set; }
            // public int[] Sequence { get; set; }
            public long Compressed { get; set; }
            public bool Usable { get; set; }

            public const int SequenceLength = 4;

            public Secret()
            {
                Number = 0;
                PriceChange = 0;
                // Sequence = [0, 0, 0, 0];
                Compressed = 0;
                Usable = false;
            }

            public Secret(long number, Secret prev, bool usable)
            {
                Number = number;
                PriceChange = (byte)((Number % 10 - prev.Number % 10) + 9);
                // Sequence = new int[SequenceLength];
                // for (int i = 0; i < SequenceLength - 1; ++i)
                // {
                //     Sequence[i] = prev.Sequence[i + 1];
                // }
                Compressed = (prev.Compressed << 8) | PriceChange;
                // Sequence[SequenceLength - 1] = PriceChange;
                Usable = usable;
            }
        }

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool findBest)
        {
            GetVariable(nameof(_SecretNumberCount), 2000, variables, out int secretNumberCount);
            List<long> secretNumbers = [.. inputs.Select(long.Parse)];
            List<long> nextSecrets = [];
            List<List<Secret>> bestSecrets = [];
            bestSecrets.AddRange([.. Enumerable.Range(1, secretNumbers.Count).Select(i => new List<Secret>())]);
            for (int i = 0; i < secretNumbers.Count; ++i)
            {
                bestSecrets[i].Add(new Secret());
                bestSecrets[i][0].Number = secretNumbers[i];
            }

            for (int i = 0; i < secretNumberCount; ++i)
            {
                int curMonkeyIdx = 0;
                foreach (long secretNumber in secretNumbers)
                {
                    // step 1
                    long working = secretNumber * 64;
                    working ^= secretNumber;
                    working %= 16777216;
                    long newSecretNumber = working;

                    // step 2
                    working /= 32;
                    working ^= newSecretNumber;
                    working %= 16777216;
                    newSecretNumber = working;

                    // step 3
                    working *= 2048;
                    working ^= newSecretNumber;
                    working %= 16777216;

                    nextSecrets.Add(working);

                    if (findBest)
                    {
                        Secret prevSecret = bestSecrets[curMonkeyIdx].Last();
                        bestSecrets[curMonkeyIdx].Add(new Secret(working, prevSecret, i >= (Secret.SequenceLength - 1)));
                    }

                    //Log($"{secretNumber} -> {working}");
                    ++curMonkeyIdx;
                }
                secretNumbers = [.. nextSecrets];
                nextSecrets.Clear();
            }

            if (!findBest)
            {
                return secretNumbers.Sum().ToString();
            }

            // loop through each best secret
            //    find the first pattern for 9-1
            //    check the rest

            for (int i = 0; i < bestSecrets.Count; ++i)
            {
                bestSecrets[i] = [.. bestSecrets[i].OrderByDescending(s => s.Number % 10)];
            }

            return "";
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}