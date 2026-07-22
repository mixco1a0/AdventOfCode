using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC._2016
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
                    Output = "1985",
                    RawInput =
@"ULL
RRDDD
LURDL
UUUUD"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "5DB3",
                    RawInput =
@"ULL
RRDDD
LURDL
UUUUD"
                },
            ];
            return testData;
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, string[] numPad, Base.Vec2 start)
        {
            Base.Range range = new(0, numPad.First().Length - 1);
            StringBuilder code = new();
            int x = start.X, y = start.Y;
            foreach (string input in inputs)
            {
                foreach (char c in input)
                {
                    Base.Vec2 old = new(x, y);
                    switch (c)
                    {
                        case 'U':
                            y = Math.Max(range.Min, y - 1);
                            break;
                        case 'D':
                            y = Math.Min(range.Max, y + 1);
                            break;
                        case 'L':
                            x = Math.Max(range.Min, x - 1);
                            break;
                        case 'R':
                            x = Math.Min(range.Max, x + 1);
                            break;
                    }
                    if (numPad[y][x] == ' ')
                    {
                        x = old.X;
                        y = old.Y;
                    }
                }
                code.Append(numPad[y][x]);
            }
            return code.ToString();
        }

        static readonly string[] numberPad1 = ["123", "456", "789"];

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, numberPad1, new Base.Vec2(1, 1));

        static readonly string[] numberPad2 = ["  1  ", " 234 ", "56789", " ABC ", "  D  "];

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, numberPad2, new Base.Vec2(0, 2));
    }
}