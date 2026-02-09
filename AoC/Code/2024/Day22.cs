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
            public long Price { get => Number % 10; }
            public long PriceChange { get; set; }
            public ulong Compressed { get; set; }
            public string CString { get => Compressed.ToString("X8"); }
            public bool Usable { get; set; }

            public const int SequenceLength = 4;

            public Secret()
            {
                Number = 0;
                PriceChange = 0;
                Compressed = 0;
                Usable = false;
            }

            public Secret(long number, Secret prev, bool usable)
            {
                Number = number;
                PriceChange = Price - prev.Price;
                Compressed = ((prev.Compressed << 8) | (byte)(PriceChange + 9)) & 0xffffffff;
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

            Dictionary<ulong, int> frequency = [];
            for (int i = 0; i < bestSecrets.Count; ++i)
            {
                var curSecretList = bestSecrets[i].Where(s => s.Usable).Select((s, i) => new { Idx = i, Sec = s }).ToList();
                var pairList = curSecretList.Where(pair => pair.Sec.Price == 0).ToList();
                foreach (var pl in pairList)
                {
                    curSecretList.RemoveAll(pair => pair.Sec.Compressed == pl.Sec.Compressed && pair.Idx >= pl.Idx);
                }
                bestSecrets[i] = [.. curSecretList.Select(pair => pair.Sec)];
                foreach (ulong curCompressed in bestSecrets[i].Select(s => s.Compressed))
                {
                    if (frequency.TryGetValue(curCompressed, out int value))
                    {
                        frequency[curCompressed] = value + 1;
                    }
                    else
                    {
                        frequency[curCompressed] = 1;
                    }
                }
            }
            IEnumerable<ulong> compressed = frequency.Select(pair => pair).OrderByDescending(pair => pair.Value).Select(pair => pair.Key);
            foreach (var p in frequency.Select(pair => pair).OrderByDescending(pair => pair.Value).Take(10))
            {
                Log($"{p.Key:X8} has {p.Value} instance");
            }

            long curBest = long.MinValue;
            foreach (ulong c in compressed)
            {
                if (secretNumbers.Count > 10)
                {
                    // short circuit
                    int curFreq = frequency[c];
                    if (curFreq * 5 < curBest)
                    {
                        break;
                    }
                }

                long best = bestSecrets.Select(list => list.FirstOrDefault(s => s.Compressed == c)).Where(s => s != null).Select(s => s.Price).Sum();
                curBest = Math.Max(best, curBest);

                if (curBest == best)
                {
                    Log($"new max! {curBest}");
                }
            }

            return curBest.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}