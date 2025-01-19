using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoC.Util;

namespace AoC._2024
{
    class Day21 : Core.Day
    {
        public Day21() { }

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
                    Output = "126384",
                    RawInput =
@"029A
980A
179A
456A
379A"
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

        private enum KeypadType
        {
            Number,
            Directional
        }

        private readonly char A = 'A';

        private readonly string[] NumberKeypad = ["789", "456", "123", " 0A"];
        private readonly string[] DirectionalKeypad = [" ^A", "<v>"];

        private record FromTo(KeypadType Type, char From, char To);
        private Dictionary<FromTo, string> InstructionCache;

        private string GetInstruction(FromTo fromTo, Base.Grid2Char grid)
        {
            if (InstructionCache.TryGetValue(fromTo, out string value))
            {
                return value;
            }

            Base.Vec2 start = new(), end = new();
            foreach (Base.Vec2 vec2 in grid)
            {
                if (grid[vec2] == fromTo.From)
                {
                    start = vec2;
                }
                else if (grid[vec2] == fromTo.To)
                {
                    end = vec2;
                }
            }

            Dictionary<Base.Vec2, string> history = [];
            history[start] = "";

            Queue<Base.Vec2> queue = new();
            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                Base.Vec2 cur = queue.Dequeue();
                if (cur.Equals(end))
                {
                    StringBuilder sb = new(history[end]);
                    sb.Append(A);
                    return sb.ToString();
                }

                foreach (Util.Grid2.Dir dir in Util.Grid2.Iter.Cardinal)
                {
                    Base.Vec2 next = cur + Util.Grid2.Map.Neighbor[dir];
                    if (!grid.Contains(next) || history.ContainsKey(next) || grid[next] == ' ')
                    {
                        continue;
                    }

                    StringBuilder sb = new(history[cur]);
                    sb.Append(Util.Grid2.Map.SimpleArrow[dir]);
                    history[next] = sb.ToString();
                    queue.Enqueue(next);
                }
            }

            return string.Empty;
        }

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool _)
        {
            Base.Grid2Char numberGrid = new(NumberKeypad);
            Base.Vec2 numberAVec2 = new();
            foreach (Base.Vec2 vec2 in numberGrid)
            {
                if (numberGrid[vec2] == 'A')
                {
                    numberAVec2 = vec2;
                    break;
                }
            }

            Base.Grid2Char directionalGrid = new(DirectionalKeypad);
            Base.Vec2 directionalAVec2 = new();
            foreach (Base.Vec2 vec2 in directionalGrid)
            {
                if (directionalGrid[vec2] == 'A')
                {
                    directionalAVec2 = vec2;
                    break;
                }
            }

            InstructionCache = [];

            // get instructions required to push buttons
            List<string> instructions = [];
            foreach (string input in inputs)
            {
                StringBuilder sb = new();
                char curInstruction = A;
                for (int i = 0; i < input.Length; ++i)
                {
                    FromTo ft = new(KeypadType.Number, curInstruction, input[i]);
                    string instruction = GetInstruction(ft, numberGrid);
                    sb.Append(instruction);
                    curInstruction = input[i];
                }
                instructions.Add(sb.ToString());

                // Log($"{input} => {sb.ToString()}");
            }

            // convert instructions into directional 3 times
            for (int j = 0; j < 2; ++j)
            {
                List<string> tempInstructions = [.. instructions];
                instructions.Clear();
                foreach (string input in tempInstructions)
                {
                    StringBuilder sb = new();
                    char curInstruction = A;
                    for (int i = 0; i < input.Length; ++i)
                    {
                        FromTo ft = new(KeypadType.Directional, curInstruction, input[i]);
                        string instruction = GetInstruction(ft, directionalGrid);
                        sb.Append(instruction);
                        curInstruction = input[i];
                    }
                    instructions.Add(sb.ToString());
                    // Log($"{input} => {sb.ToString()}");
                }
            }

            int sum = 0;
            for (int i = 0; i < inputs.Count; ++i)
            {
                int code = int.Parse(inputs[i][0..^1]);
                sum += code * instructions[i].Length;
            }

            return sum.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}