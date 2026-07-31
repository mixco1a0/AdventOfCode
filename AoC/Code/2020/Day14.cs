using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2020
{
    class Day14 : Core.Day
    {
        public Day14() { }

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
                    Output = "165",
                    RawInput =
@"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "208",
                    RawInput =
@"mask = 000000000000000000000000000000X1001X
mem[42] = 100
mask = 00000000000000000000000000000000X0XX
mem[26] = 1"
                },
            ];
            return testData;
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            Dictionary<string, string> memory = [];
            List<KeyValuePair<char, int>> masks = [];
            foreach (string input in inputs)
            {
                if (input.Contains("mask"))
                {
                    List<string> split = [.. Util.String.Split(input, " =")];

                    masks = [.. split[1].ToCharArray().Select((digit, index) => new { Digit = digit, Index = index }).Where(pair => pair.Digit != 'X').Select(pair => new KeyValuePair<char, int>(pair.Digit, pair.Index))];
                }
                else
                {
                    List<string> split = [.. Util.String.Split(input, " []=")];
                    string val = Convert.ToString(long.Parse(split[2]), 2).ToString().PadLeft(36, '0');
                    char[] chars = val.ToCharArray();
                    foreach (var pair in masks)
                    {
                        chars[pair.Value] = pair.Key;
                    }
                    memory[split[1]] = string.Join("", chars);
                }
            }

            long sum = 0;
            foreach (var pair in memory)
            {
                sum += Convert.ToInt64(pair.Value, 2);
            }
            return sum.ToString();
        }

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            Dictionary<string, long> memory = [];
            List<KeyValuePair<char, int>> masks = [];
            foreach (string input in inputs)
            {
                if (input.Contains("mask"))
                {
                    List<string> split = [.. Util.String.Split(input, " =")];

                    masks = [.. split[1].ToCharArray().Select((digit, index) => new { Digit = digit, Index = index }).Select(pair => new KeyValuePair<char, int>(pair.Digit, pair.Index))];
                }
                else
                {
                    List<string> split = [.. Util.String.Split(input, " []=")];
                    char[] memAddress = Convert.ToString(long.Parse(split[1]), 2).ToString().PadLeft(36, '0').ToCharArray();
                    char[] chars = Convert.ToString(long.Parse(split[1]), 2).ToString().PadLeft(36, '0').ToCharArray();
                    foreach (var pair in masks)
                    {
                        if (pair.Key == '0')
                        {
                            continue;
                        }

                        chars[pair.Value] = pair.Key;
                        if (pair.Key == '1')
                        {
                            memAddress[pair.Value] = '1';
                        }
                    }

                    var allXs = chars.Select((c, idx) => new { Letter = c, Index = idx }).Where(pair => pair.Letter == 'X').Select(pair => new KeyValuePair<char, int>(pair.Letter, pair.Index)).ToList();
                    long max = (long)Math.Pow(2, allXs.Count);
                    for (int i = 0; i < max; ++i)
                    {
                        string curReplace = Convert.ToString(i, 2).PadLeft(allXs.Count, '0');
                        char[] curAddress = string.Join("", memAddress).ToCharArray();
                        for (int j = 0; j < curReplace.Length; ++j)
                        {
                            curAddress[allXs[j].Value] = curReplace[j];
                        }
                        memory[string.Join("", curAddress)] = long.Parse(split[2]);
                    }
                }
            }

            long sum = 0;
            foreach (var pair in memory)
            {
                sum += pair.Value;
            }
            return sum.ToString();
        }
    }
}