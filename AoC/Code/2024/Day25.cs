using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2024
{
    class Day25 : Core.Day
    {
        public Day25() { }

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
                    Output = "3",
                    RawInput =
@"#####
.####
.####
.####
.#.#.
.#...
.....

.....
#....
#....
#...#
#.#.#
#.###
#####

#####
##.##
.#.##
...##
...#.
...#.
.....

.....
.....
#.#..
###..
###.#
###.#
#####

.....
.....
.....
#....
#.#..
#.#.#
#####"
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

        static readonly string LockInputStart = "#####";
        static readonly string KeyInputStart = ".....";
        static readonly int MaxPinHeight = 5;

        private void ParseInput(List<string> inputs, out List<int[]> locks, out List<int[]> keys)
        {
            locks = [];
            keys = [];

            bool parsingLocks = true;
            bool newParse = true;

            int[] curInfo = [0, 0, 0, 0, 0];
            int curHeight = 0;
            foreach (string input in inputs)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    newParse = true;
                    continue;
                }

                bool endParse = !newParse && (input == LockInputStart || input == KeyInputStart) && ((parsingLocks && curHeight >= MaxPinHeight) || (!parsingLocks && curHeight == 0));
                if (newParse)
                {
                    newParse = false;
                    if (input == LockInputStart)
                    {
                        curHeight = 0;
                        parsingLocks = true;
                    }
                    else if (input == KeyInputStart)
                    {
                        curHeight = MaxPinHeight;
                        parsingLocks = false;
                    }
                    curInfo = [curHeight, curHeight, curHeight, curHeight, curHeight];
                }
                else if (endParse)
                {
                    if (parsingLocks)
                    {
                        locks.Add([.. curInfo]);
                    }
                    else
                    {
                        keys.Add([.. curInfo]);
                    }
                }
                else
                {
                    if (parsingLocks)
                    {
                        ++curHeight;
                        for (int i = 0; i < input.Length; ++i)
                        {
                            curInfo[i] += input[i] == '#' ? 1 : 0;
                        }
                    }
                    else
                    {
                        --curHeight;
                        for (int i = 0; i < input.Length; ++i)
                        {
                            curInfo[i] -= input[i] == '.' ? 1 : 0;
                        }
                    }
                }

            }
        }

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool _)
        {
            ParseInput(inputs, out List<int[]> locks, out List<int[]> keys);
            ulong combos = 0;
            foreach (int[] key in keys)
            {
                foreach (int[] _lock in locks)
                {
                    bool isCombo = true;
                    for (int i = 0; i < 5 && isCombo; ++i)
                    {
                        isCombo &= (key[i] + _lock[i]) <= MaxPinHeight;
                    }
                    if (isCombo)
                    {
                        ++combos;
                    }
                }
            }
            return combos.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}