using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2016
{
    class Day07 : Core.Day
    {
        public Day07() { }

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
                    Output = "3",
                    RawInput =
@"abba[mnop]qrst
qrst[mnop]abba
abcd[bddb]xyyx
aaaa[qwer]tyui
ioxxoj[asdfgh]zxcvbn"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "4",
                    RawInput =
@"aba[bab]xyz
xyx[xyx]xyx
aaa[kek]eke
zazbz[bzb]cdb
xyx[xyxy]xyx"
                },
            ];
            return testData;
        }

        private List<string> GetABBA(string input)
        {
            List<string> found = [];
            for (int i = 0; i < input.Length - 3; ++i)
            {
                if (input[i] == input[i + 3] && input[i + 1] == input[i + 2] && input[i] != input[i + 1])
                {
                    found.Add(input[i..(i + 2)]);
                }
            }
            return found;
        }

        private List<string> GetABA(string input)
        {
            List<string> found = [];
            for (int i = 0; i < input.Length - 2; ++i)
            {
                if (input[i] == input[i + 2] && input[i] != input[i + 1])
                {
                    found.Add(input[i..(i + 2)]);
                }
            }
            return found;
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, Func<string, List<string>> patternFunc, bool checkSupernet)
        {
            int tlsSupportCount = 0;
            foreach (string input in inputs)
            {
                List<string> patterns = [];
                List<string> revPatterns = [];
                string[] split = Util.String.Split(input.Replace("[", "[|"), "[]");
                foreach (string s in split)
                {
                    bool supernetSeq = s.First() == '|';
                    List<string> curPatterns = patternFunc(s);
                    if (!supernetSeq && curPatterns.Count > 0)
                    {
                        patterns.AddRange(curPatterns);
                    }
                    else if (supernetSeq && curPatterns.Count > 0)
                    {
                        curPatterns.ForEach(rp => revPatterns.Add($"{rp[1]}{rp[0]}"));
                    }
                }

                if (patterns.Count > 0)
                {
                    if (checkSupernet)
                    {
                        if (revPatterns.Count == 0)
                        {
                            continue;
                        }

                        if (!patterns.Intersect(revPatterns).Any())
                        {
                            continue;
                        }
                    }
                    else if (revPatterns.Count != 0)
                    {
                        continue;
                    }
                    ++tlsSupportCount;
                }
            }
            return tlsSupportCount.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, GetABBA, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, GetABA, true);
    }
}