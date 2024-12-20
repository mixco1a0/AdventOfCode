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
                    return "v3";
                case Core.Part.Two:
                    return "v3";
                default:
                    return base.GetSolutionVersion(part);
            }
        }

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

        private Dictionary<char, Base.Vec2> Movements = new Dictionary<char, Base.Vec2>()
        {
            {L, new Base.Vec2(-1, 0)},
            {R, new Base.Vec2(1, 0)},
            {U, new Base.Vec2(0, 1)},
            {D, new Base.Vec2(0, -1)},
        };

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool usingRobotSanta)
        {
            bool santaMove = true;
            Base.Vec2 santaCoords = new Base.Vec2();
            Base.Vec2 robotCoords = new Base.Vec2();

            HashSet<Base.Vec2> visitedCoords = new HashSet<Base.Vec2>();
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