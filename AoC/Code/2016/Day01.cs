using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2016
{
    class Day01 : Core.Day
    {
        public Day01() { }

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
                    Output = "5",
                    RawInput =
@"R2, L3"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Output = "2",
                    RawInput =
@"R2, R2, R2"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Output = "12",
                    RawInput =
@"R5, L5, R5, R3"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "4",
                    RawInput =
@"R8, R4, R4, R8"
                },
            ];
            return testData;
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool segmentCheck)
        {
            // used for segment checks
            List<Base.Ray2> visited = [];
            Base.Vec2 prev = new(0, 0);

            int coordX = 0, coordY = 0, curDirection = 0;
            string[] input = Util.String.Split(inputs[0], " ,");
            foreach (string i in input)
            {
                curDirection += (i[0] == 'R' ? 1 : -1);
                if (curDirection < 0)
                {
                    curDirection += 4;
                }
                switch (curDirection % 4)
                {
                    case 0:
                        coordY += int.Parse(i[1..]);
                        break;
                    case 1:
                        coordX += int.Parse(i[1..]);
                        break;
                    case 2:
                        coordY -= int.Parse(i[1..]);
                        break;
                    case 3:
                        coordX -= int.Parse(i[1..]);
                        break;
                }

                if (segmentCheck)
                {
                    Base.Ray2 cur = Base.Ray2.FromPos(prev, new(coordX, coordY));
                    // Core.Log.WriteLine(Core.Log.ELevel.Spam, $"({cur.Pos.X,4},{cur.Pos.Y,4}) -> ({cur.Next.X,4}, {cur.Next.Y,4})");
                    Base.Vec2 intersection = null;
                    // check for intersection
                    foreach (Base.Ray2 visit in visited.Take(visited.Count - 1))
                    {
                        if (cur.Intersects(visit, out intersection))
                        {
                            break;
                        }
                    }

                    if (intersection != null)
                    {
                        coordX = intersection.X;
                        coordY = intersection.Y;
                        break;
                    }
                    
                    visited.Add(cur);
                    prev = cur.Next;
                }
            }
            return (Math.Abs(coordX) + Math.Abs(coordY)).ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}