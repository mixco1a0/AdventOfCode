using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2024
{
    class Day14 : Core.Day
    {
        public Day14() { }

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
                    Output = "12",
                    Variables = new Dictionary<string, string> { { nameof(_TilesWide), "11" }, { nameof(_TilesTall), "7" } },
                    RawInput =
@"p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=2,4 v=2,-3
p=9,5 v=-3,-3"
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
        private static int _TilesWide { get; }
        private static int _TilesTall { get; }
#pragma warning restore IDE1006 // Naming Styles

        public static Base.Ray2 Parse(string input)
        {
            int[] split = Util.String.Split(input, "pv=, ").Where(i => int.TryParse(i, out int _)).Select(int.Parse).ToArray();
            Base.Vec2 start = new(split[0], split[1]);
            Base.Vec2 vel = new(split[2], split[3]);
            return Base.Ray2.FromVel(start, vel);
        }

        public static int GetQuadrant(int tilesWide, int tilesTall, ref Base.Vec2 pos)
        {
            int x = pos.X % tilesWide;
            if (x < 0)
            {
                x += tilesWide;
            }
            pos.X = x;

            int y = pos.Y % tilesTall;
            if (y < 0)
            {
                y += tilesTall;
            }
            pos.Y = y;

            int nonX = (tilesWide - 1) / 2;
            int nonY = (tilesTall - 1) / 2;

            if (x == nonX || y == nonY)
            {
                return 0;
            }

            if (x < nonX)
            {
                if (y < nonY)
                {
                    return 1;
                }
                else
                {
                    return 3;
                }
            }
            else
            {
                if (y < nonX)
                {
                    return 2;
                }
                else
                {
                    return 4;
                }
            }
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool _)
        {
            List<string> test = ["123", "456"];
            Base.Grid2Int testG = new(test);

            GetVariable(nameof(_TilesWide), 101, variables, out int tilesWide);
            GetVariable(nameof(_TilesTall), 103, variables, out int tilesTall);
            Base.Ray2[] robots = inputs.Select(Parse).ToArray();
            int[] quadrants = [0, 0, 0, 0, 0];
            Base.Grid2Int grid = new(tilesWide, tilesTall);
            foreach (Base.Ray2 robot in robots)
            {
                Base.Vec2 pos = robot.Tick(0);
                ++grid[pos.X, pos.Y];
            }
            grid.Print(Core.Log.ELevel.Spam);

            grid = new(tilesWide, tilesTall);
            foreach (Base.Ray2 robot in robots)
            {
                Base.Vec2 pos = robot.Tick(100);
                int quadrant = GetQuadrant(tilesWide, tilesTall, ref pos);
                ++grid[pos.X, pos.Y];
                ++quadrants[quadrant];
            }

            grid.Print(Core.Log.ELevel.Spam);

            int mult = 1;
            foreach (int q in quadrants.Skip(1))
            {
                mult *= q;
            }
            return mult.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);
            // TOO HIGH - 212266600

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}