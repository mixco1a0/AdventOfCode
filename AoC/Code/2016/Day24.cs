using System;
using System.Collections.Generic;

namespace AoC._2016
{
    class Day24 : Core.Day
    {
        public Day24() { }

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
                Output = "14",
                RawInput =
@"###########
#0.1.....2#
#.#######.#
#4.......3#
###########"
            });
            testData.Add(new Core.TestDatum
            {
                TestPart = Core.Part.Two,
                Output = "",
                RawInput =
@""
            });
            return testData;
        }

        private char WallChar = '#';
        private char OpenChar = '.';
        private char StartChar = '0';

        private record Path(Base.Pos2 Coords, int Steps) { }

        private int PathTo(char[][] grid, Base.Pos2 max, Base.Pos2 start, char target)
        {
            Queue<Path> pendingChecks = new Queue<Path>();
            pendingChecks.Enqueue(new Path(start, 0));

            HashSet<Base.Pos2> history = new HashSet<Base.Pos2>();
            history.Add(start);

            while (pendingChecks.Count > 0)
            {
                Path cur = pendingChecks.Dequeue();
                if (grid[cur.Coords.Y][cur.Coords.X] == target)
                {
                    return cur.Steps;
                }

                Base.Pos2[] movements = new Base.Pos2[] { new Base.Pos2(0, -1), new Base.Pos2(-1, 0), new Base.Pos2(0, 1), new Base.Pos2(1, 0) };
                foreach (Base.Pos2 movement in movements)
                {
                    Base.Pos2 nextMove = cur.Coords + movement;
                    if (nextMove.X >= 0 && nextMove.X < max.X && nextMove.Y >= 0 && nextMove.Y < max.Y)
                    {
                        if (grid[nextMove.Y][nextMove.X] == WallChar || history.Contains(nextMove))
                        {
                            continue;
                        }

                        pendingChecks.Enqueue(new Path(nextMove, cur.Steps + 1));
                        history.Add(nextMove);
                    }
                }
            }

            return (int)short.MaxValue;
        }

        private int[] GeneratePaths(char[][] grid, Base.Pos2 max, char target, int totalTargets)
        {
            // get starting position
            Base.Pos2 start = new Base.Pos2(-1, -1);
            for (int y = 0; y < max.Y && start.Y < 0; ++y)
            {
                for (int x = 0; x < max.X; ++x)
                {
                    if (grid[y][x] == target)
                    {
                        start = new Base.Pos2(x, y);
                        break;
                    }
                }
            }

            // get starting target to every other target
            int[] fromTargetTo = new int[totalTargets];
            for (int curTarget = 0; curTarget < totalTargets; ++curTarget)
            {
                char curTargetChar = curTarget.ToString()[0];
                if (curTargetChar == target)
                {
                    fromTargetTo[curTarget] = 0;
                    continue;
                }

                fromTargetTo[curTarget] = PathTo(grid, max, start, curTargetChar);
            }
            return fromTargetTo;
        }

        private int FindShortestPath(int[][] toAllTargets, int totalTargets, int prevTarget, string usedTargets, int pathLength, bool returnHome)
        {
            if (usedTargets.Length == totalTargets)
            {
                if (returnHome)
                {
                    return pathLength + toAllTargets[prevTarget][0];
                }
                else
                {
                    return pathLength;
                }
            }

            int shortestPath = int.MaxValue;
            for (int curTarget = 0; curTarget < totalTargets; ++curTarget)
            {
                char curTargetChar = curTarget.ToString()[0];
                if (usedTargets.Contains(curTargetChar))
                {
                    continue;
                }

                int newLength = pathLength + toAllTargets[prevTarget][curTarget];
                shortestPath = Math.Min(shortestPath, FindShortestPath(toAllTargets, totalTargets, curTarget, usedTargets + curTarget.ToString(), newLength, returnHome));
            }

            return shortestPath;
        }

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool returnHome)
        {
            Base.Pos2 max = new Base.Pos2(inputs[0].Length, inputs.Count);
            char[][] grid = new char[inputs.Count][];
            Base.Pos2 start = new Base.Pos2();
            for (int i = 0; i < inputs.Count; ++i)
            {
                grid[i] = inputs[i].ToCharArray();
                int idx = inputs[i].IndexOf(StartChar);
                if (idx >= 0)
                {
                    start.Y = i;
                    start.X = idx;
                }
            }

            string allTargets = string.Join(string.Empty, inputs).Replace($"{WallChar}", string.Empty).Replace($"{OpenChar}", string.Empty);
            int[][] toAllTargets = new int[allTargets.Length][];
            foreach (char target in allTargets)
            {
                toAllTargets[int.Parse($"{target}")] = GeneratePaths(grid, max, target, allTargets.Length);
            }
            return FindShortestPath(toAllTargets, allTargets.Length, 0, "0", 0, returnHome).ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}