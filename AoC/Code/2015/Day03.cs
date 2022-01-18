using System.Collections.Generic;
using System.Linq;

namespace AoC._2015
{
    class Day03 : Core.Day
    {
        public Day03() { }

        public override string GetSolutionVersion(Core.Part part)
        {
            switch (part)
            {
                case Core.Part.One:
                    return "v2";
                case Core.Part.Two:
                    return "v2";
                default:
                    return base.GetSolutionVersion(part);
            }
        }

        public override bool SkipTestData => true;

        protected override List<Core.TestDatum> GetTestData()
        {
            List<Core.TestDatum> testData = new List<Core.TestDatum>();
            testData.Add(new Core.TestDatum
            {
                TestPart = Core.Part.One,
                Output = "2",
                RawInput =
@">"
            });
            testData.Add(new Core.TestDatum
            {
                TestPart = Core.Part.One,
                Output = "4",
                RawInput =
@"^>v<"
            });
            testData.Add(new Core.TestDatum
            {
                TestPart = Core.Part.One,
                Output = "2",
                RawInput =
@"^v^v^v^v^v"
            });
            testData.Add(new Core.TestDatum
            {
                TestPart = Core.Part.Two,
                Output = "3",
                RawInput =
@"^v"
            });
            testData.Add(new Core.TestDatum
            {
                TestPart = Core.Part.Two,
                Output = "3",
                RawInput =
@"^>v<"
            });
            testData.Add(new Core.TestDatum
            {
                TestPart = Core.Part.Two,
                Output = "11",
                RawInput =
@"^v^v^v^v^v"
            });
            return testData;
        }

        private const char L = '<';
        private const char R = '>';
        private const char U = '^';
        private const char D = 'v';

        private Dictionary<char, Base.Point> Movements = new Dictionary<char, Base.Point>()
        {
            {L, new Base.Point(-1, 0)},
            {R, new Base.Point(1, 0)},
            {U, new Base.Point(0, 1)},
            {D, new Base.Point(0, -1)},
        };

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool usingRobotSanta)
        {
            bool santaMove = true;
            Base.Point santaCoords = new Base.Point();
            Base.Point robotCoords = new Base.Point();

            HashSet<Base.Point> visitedCoords = new HashSet<Base.Point>();
            visitedCoords.Add(santaCoords);
            foreach (char c in string.Join(' ', inputs))
            {
                if (santaMove || !usingRobotSanta)
                {
                    santaCoords += Movements[c];
                    visitedCoords.Add(santaCoords);
                }
                else
                {
                    robotCoords += Movements[c];
                    visitedCoords.Add(robotCoords);
                }
                santaMove = !santaMove;
            }
            return visitedCoords.Count().ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}