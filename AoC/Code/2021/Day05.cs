using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC._2021
{
    class Day05 : Core.Day
    {
        public Day05() { }

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
                    Output = "5",
                    RawInput =
@"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "12",
                    RawInput =
@"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2"
                },
            ];
            return testData;
        }

        private static class SegmentExtension
        {
            public static Base.Segment Parse(string input)
            {
                int[] vals = [.. input.Split(", ->".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)];
                Base.Vec2 a = new(vals[0], vals[1]);
                Base.Vec2 b = new(vals[2], vals[3]);
                return new Base.Segment(a, b);
            }
        }

        private enum Direction
        {
            NegativeX,
            NegativeY,
            DiagonalUp,
            DiagonalDown,
        }

        private Dictionary<Direction, Base.Vec2> Movement = new()
        {
            { Direction.NegativeX, new Base.Vec2(-1, 0) },
            { Direction.NegativeY, new Base.Vec2(0, -1) },
            { Direction.DiagonalUp, new Base.Vec2(-1, -1) },
            { Direction.DiagonalDown, new Base.Vec2(-1, 1) },
        };

        private void PrintGrid(Dictionary<Base.Vec2, int> grid)
        {
            HashSet<Base.Vec2> coords = [.. grid.Keys];
            int maxX = coords.Select(c => c.X).Max();
            int maxY = coords.Select(c => c.Y).Max();
            for (int y = 0; y <= maxY; ++y)
            {
                StringBuilder sb = new();
                sb.AppendFormat("{0,3} | ", y);
                for (int x = 0; x <= maxX; ++x)
                {
                    Base.Vec2 cur = new(x, y);
                    if (grid.ContainsKey(cur))
                    {
                        sb.Append($"{grid[cur],1}");
                    }
                    else
                    {
                        sb.Append($".");
                    }
                }
                Log(sb.ToString());
                sb.Clear();
            }
            Log(string.Empty);
        }

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool checkDiagonals)
        {
            Dictionary<Base.Vec2, int> overlaps = [];
            Action<int, Direction, Base.Vec2> CheckCoords = (count, dir, start) =>
            {
                for (int i = 0; i <= count; ++i)
                {
                    if (!overlaps.ContainsKey(start))
                    {
                        overlaps[start] = 0;
                    }
                    ++overlaps[start];
                    start += Movement[dir];
                }
            };

            Base.Segment[] segments = [.. inputs.Select(SegmentExtension.Parse)];
            foreach (Base.Segment segment in segments)
            {
                if (segment.A.X == segment.B.X)
                {
                    CheckCoords(Math.Abs(segment.A.Y - segment.B.Y), Direction.NegativeY, segment.A.Y > segment.B.Y ? segment.A : segment.B);
                }
                else if (segment.A.Y == segment.B.Y)
                {
                    CheckCoords(Math.Abs(segment.A.X - segment.B.X), Direction.NegativeX, segment.A.X > segment.B.X ? segment.A : segment.B);
                }
                else if (checkDiagonals)
                {
                    Base.Vec2 start = segment.A.X > segment.B.X ? segment.A : segment.B;
                    Base.Vec2 end = start.Equals(segment.A) ? segment.B : segment.A;
                    if (end.Y < start.Y)
                    {
                        CheckCoords(Math.Abs(segment.A.X - segment.B.X), Direction.DiagonalUp, start);
                    }
                    else
                    {
                        CheckCoords(Math.Abs(segment.A.X - segment.B.X), Direction.DiagonalDown, start);
                    }
                }
            }
            return overlaps.Where(p => p.Value >= 2).Count().ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}