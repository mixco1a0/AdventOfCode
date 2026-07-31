using System.Collections.Generic;
using System.Linq;

namespace AoC._2016
{
    class Day13 : Core.Day
    {
        public Day13() { }

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
                    Variables = new Dictionary<string, string>() { { nameof(_TargetX), "7" }, { nameof(_TargetY), "4" } },
                    Output = "11",
                    RawInput =
@"10"
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

#pragma warning disable IDE1006 // Naming Styles
        private static string _TargetX { get; }
        private static string _TargetY { get; }
#pragma warning restore IDE1006 // Naming Styles

        private static ulong GetId(int x, int y)
        {
            ulong id = (uint)y;
            id <<= 32;
            return id | (ulong)(uint)x;
        }

        private static bool IsOpen(uint x, uint y, uint magicNumber)
        {
            uint isWall = (x * x) + (3 * x) + (2 * x * y) + y + (y * y) + magicNumber;
            int bits = 0;
            for (uint i = 0, bit = 1; i < 32; ++i, bit <<= 1)
            {
                bits += ((isWall & bit) == 0 ? 0 : 1);
            }
            return bits % 2 == 0;
        }

        private record PointWalk(Base.Vec2 Point, uint Distance) { }

        private static uint WalkPath(Queue<PointWalk> points, Base.Vec2 target, uint magicNumber, int maxDistance)
        {
            HashSet<ulong> visited = [];
            uint walkingPoints = 0;
            while (points.Count > 0)
            {
                PointWalk pointWalk = points.Dequeue();
                Base.Vec2 point = pointWalk.Point;

                // check if this is the target
                if (maxDistance <= 0 && point.X == target.X && point.Y == target.Y)
                {
                    return pointWalk.Distance;
                }

                if (!IsOpen((uint)point.X, (uint)point.Y, magicNumber))
                {
                    continue;
                }

                if (visited.Contains(GetId(point.X, point.Y)))
                {
                    continue;
                }
                visited.Add(GetId(point.X, point.Y));

                if (maxDistance > 0)
                {
                    if (pointWalk.Distance <= maxDistance)
                    {
                        ++walkingPoints;
                    }
                    else
                    {
                        continue;
                    }
                }

                // add new points
                points.Enqueue(new PointWalk(new Base.Vec2(point.X + 1, point.Y), pointWalk.Distance + 1));
                points.Enqueue(new PointWalk(new Base.Vec2(point.X, point.Y + 1), pointWalk.Distance + 1));
                if (point.X > 0)
                {
                    points.Enqueue(new PointWalk(new Base.Vec2(point.X - 1, point.Y), pointWalk.Distance + 1));
                }
                if (point.Y > 0)
                {
                    points.Enqueue(new PointWalk(new Base.Vec2(point.X, point.Y - 1), pointWalk.Distance + 1));
                }

            }
            return walkingPoints;
        }

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool findMaxLocations)
        {
            GetVariable(nameof(_TargetX), 31, variables, out int targetX);
            GetVariable(nameof(_TargetY), 39, variables, out int targetY);

            uint magicNumber = uint.Parse(inputs.First());
            Queue<PointWalk> points = new();
            points.Enqueue(new PointWalk(new Base.Vec2(1, 1), 0));
            return WalkPath(points, new Base.Vec2(targetX, targetY), magicNumber, findMaxLocations ? 50 : 0).ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}