using System.Collections.Generic;
using System.Linq;

namespace AoC._2021
{
    class Day10 : Core.Day
    {
        public Day10() { }

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
                    Output = "26397",
                    RawInput =
@"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "288957",
                    RawInput =
@"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]"
                },
            ];
            return testData;
        }

        Dictionary<char, char> OpenToClose = new() { { '(', ')' }, { '[', ']' }, { '{', '}' }, { '<', '>' } };
        Dictionary<char, long> Points = new()
        {
            { ')', 3 },
            { ']', 57 },
            { '}', 1197 },
            { '>', 25137 },
            { '(', 1 },
            { '[', 2 },
            { '{', 3 },
            { '<', 4 },
        };

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool scoreCorrupt)
        {
            string allOpen = string.Join(string.Empty, OpenToClose.Keys);
            long score = 0;
            List<long> scores = [];
            foreach (string input in inputs)
            {
                Stack<char> opened = new();
                foreach (char i in input)
                {
                    if (allOpen.Contains(i))
                    {
                        opened.Push(i);
                    }
                    else
                    {
                        if (opened.Count == 0 || i != OpenToClose[opened.Peek()])
                        {
                            score += Points[i];
                            opened.Clear();
                            break;
                        }
                        else
                        {
                            opened.Pop();
                        }
                    }
                }

                // not corrupt
                if (!scoreCorrupt && opened.Count > 0)
                {
                    long completionScore = 0;
                    string completion = string.Join(string.Empty, opened);
                    foreach (char c in completion)
                    {
                        completionScore *= 5;
                        completionScore += Points[c];
                    }
                    scores.Add(completionScore);
                }
            }
            
            if (scoreCorrupt)
            {
                return score.ToString();
            }
            scores.Sort();
            int idx = (scores.Count() - 1) / 2;
            return scores[idx].ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);
    }
}