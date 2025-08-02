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
                // Core.Part.One => "v1",
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
                    Output = "",
                    RawInput =
@""
                },
            ];
            return testData;
        }

#pragma warning disable IDE1006 // Naming Styles
        private static int _SecretNumberCount { get; }
#pragma warning restore IDE1006 // Naming Styles

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool _)
        {
            GetVariable(nameof(_SecretNumberCount), 2000, variables, out int secretNumberCount);
            List<long> secretNumbers = [.. inputs.Select(long.Parse)];
            List<long> nextSecrets = [];
            for (int i = 0; i < secretNumberCount; ++i)
            {
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

                    //Log($"{secretNumber} -> {working}");
                }
                secretNumbers = [.. nextSecrets];
                nextSecrets.Clear();
            }
            return secretNumbers.Sum().ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}