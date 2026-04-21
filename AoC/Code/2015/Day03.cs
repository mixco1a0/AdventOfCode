using System.Collections.Generic;
using System.Linq;

namespace AoC._2015
{
    class Day03 : Core.Day
    {
        public Day03() { }

        public override string GetSolutionVersion(Core.Part part)
        {
            return part switch
            {
                Core.Part.One => "v3",
                Core.Part.Two => "v3",
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
                    Output = "2",
                    RawInput =
@">"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Output = "4",
                    RawInput =
@"^>v<"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Output = "2",
                    RawInput =
@"^v^v^v^v^v"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "3",
                    RawInput =
@"^v"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "3",
                    RawInput =
@"^>v<"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "11",
                    RawInput =
@"^v^v^v^v^v"
                },
            ];
            return testData;
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool usingRobotSanta)
        {
            bool santaMove = true;
            Base.Vec2 santaCoords = new();
            Base.Vec2 robotCoords = new();

            HashSet<Base.Vec2> visitedCoords = [santaCoords];
            foreach (char c in string.Join(' ', inputs))
            {
                Util.Grid2.Dir dir = Util.Grid2.Map.SimpleArrowFlipped[c];
                if (santaMove || !usingRobotSanta)
                {
                    santaCoords += Util.Grid2.Map.Neighbor[dir];
                    visitedCoords.Add(santaCoords);
                }
                else
                {
                    robotCoords += Util.Grid2.Map.Neighbor[dir];
                    visitedCoords.Add(robotCoords);
                }
                santaMove = !santaMove;
            }
            return visitedCoords.Count.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}