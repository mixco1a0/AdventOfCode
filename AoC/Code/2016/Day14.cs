using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AoC._2016
{
    class Day14 : Core.Day
    {
        public Day14() { }

        public override string GetSolutionVersion(Core.Part part)
        {
            return part switch
            {
                Core.Part.One => "v2",
                Core.Part.Two => "v0",// v2 is very slow ~20s
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
                    Output = "22728",
                    RawInput =
@"abc"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "22551",
                    RawInput =
@"abc"
                },
            ];
            return testData;
        }

        private readonly char InvalidChar = '-';

        private record HashCheck(char Match, long Start, long End, string Raw) { }

        private char FindMatches(string hash, int minLen, int minExtendedLen, out HashSet<char> extendedMatches)
        {
            extendedMatches = [];
            char firstMatch = InvalidChar;
            char cur = InvalidChar;
            int curMatchLen = 1;
            for (int i = 0; i < hash.Length; ++i)
            {
                if (cur != InvalidChar)
                {
                    if (cur == hash[i])
                    {
                        ++curMatchLen;
                        if (curMatchLen == minLen)
                        {
                            // only the first match can be used
                            if (firstMatch == InvalidChar)
                            {
                                firstMatch = cur;
                            }
                        }
                        else if (curMatchLen == minExtendedLen && !extendedMatches.Contains(cur))
                        {
                            // add series of extended matches
                            extendedMatches.Add(cur);
                        }
                    }
                    else
                    {
                        cur = InvalidChar;
                    }
                }

                if (cur == InvalidChar)
                {
                    cur = hash[i];
                    curMatchLen = 1;
                }
            }

            return firstMatch;
        }

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, int stretchCount)
        {
            const int MaxKeys = 64;
            string input = inputs.First();
            List<HashCheck> verifiedKeys = [];
            List<HashCheck> pendingKeys = [];
            for (long i = 0; verifiedKeys.Count < MaxKeys || pendingKeys.Count > 0; ++i)
            {
                StringBuilder sb = new(input);
                sb.Append(i);
                string encoded = sb.ToString();
                for (int s = 0; s < stretchCount + 1; ++s)
                {
                    byte[] inputBytes = Encoding.ASCII.GetBytes(encoded);
                    byte[] hashBytes = MD5.HashData(inputBytes);
                    encoded = Convert.ToHexStringLower(hashBytes);
                }

                char curMatch = FindMatches(encoded, 3, 5, out HashSet<char> extendedMatches);
                if (extendedMatches.Count != 0)
                {
                    // check for 5 in a row before adding the new one
                    for (int j = 0; j < pendingKeys.Count;)
                    {
                        HashCheck cur = pendingKeys[j];
                        if (extendedMatches.Contains(cur.Match))
                        {
                            verifiedKeys.Add(new HashCheck(cur.Match, cur.Start, i, cur.Raw));
                            // Log(Core.Log.ELevel.Spam, $"\'{cur.Match}\' @ {cur.Raw} [Idx {cur.Start,5}] [{encoded}]");
                            pendingKeys.RemoveAt(j);
                        }
                        else
                        {
                            ++j;
                        }
                    }
                }

                // add new keys as long as max verified hasn't been hit
                if (verifiedKeys.Count < MaxKeys && curMatch != InvalidChar)
                {
                    pendingKeys.Add(new HashCheck(curMatch, i, i + 1000, encoded));
                }

                // remove stale keys
                pendingKeys.RemoveAll(p => p.End <= i);
            }
            verifiedKeys.Sort((a, b) => a.Start > b.Start ? 1 : -1);
            // if (stretchCount > 0)
            // {
            //     foreach (var key in verifiedKeys)
            //     {
            //         Log(Core.Log.ELevel.Spam, $"{key}");
            //     }
            // }
            return verifiedKeys[MaxKeys - 1].Start.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, 0);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, 2016);
    }
}