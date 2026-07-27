using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2022
{
    class Day02 : Core.Day
    {
        public Day02() { }

        public override string GetSolutionVersion(Core.Part part)
        {
            return part switch
            {
                Core.Part.One => "v2",
                Core.Part.Two => "v2",
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
                    Output = "15",
                    RawInput =
@"A Y
B X
C Z"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "12",
                    RawInput =
@"A Y
B X
C Z"
                },
            ];
            return testData;
        }

        static Dictionary<char, char> Win = new() { { 'A', 'C' }, { 'B', 'A' }, { 'C', 'B' } };
        static Dictionary<char, char> Lose = Win.ToDictionary(w => w.Value, w => w.Key);
        static Dictionary<char, int> Points = new() { { 'A', 1 }, { 'B', 2 }, { 'C', 3 } };

        class Strategy : Base.Pair<char, char>
        {
            static public Strategy Parse(string input)
            {
                return new Strategy(){First = input[0], Last = (char)((int)input[2] - 23)};
            }
        }

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool input)
        {
            List<Strategy> guide = inputs.Select(Strategy.Parse).ToList();

            int score = 0;
            foreach (Strategy strat in guide)
            {
                // determine choice
                if (input)
                {
                    score += Points[strat.Last];
                }
                else
                {
                    // lose
                    if (strat.Last == 'A')
                    {
                        score += Points[Win[strat.First]];
                    }
                    // tie
                    else if (strat.Last == 'B')
                    {
                        score += Points[strat.First];
                    }
                    // win
                    else if (strat.Last == 'C')
                    {
                        score += Points[Lose[strat.First]];
                    }
                }

                // determine outcome
                if (input)
                {
                    if (strat.First == strat.Last)
                    {
                        score += 3;
                    }
                    else if (strat.First == Win[strat.Last])
                    {
                        score += 6;
                    }
                }
                else
                {
                    if (strat.Last == 'B')
                    {
                        score += 3;
                    }
                    else if (strat.Last == 'C')
                    {
                        score += 6;
                    }
                }
            }

            return score.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);
    }
}