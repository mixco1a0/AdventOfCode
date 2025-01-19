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
                    Output = "1972",
                    RawInput =
@"029A"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Output = "58800",
                    RawInput =
@"980A"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Output = "12172",
                    RawInput =
@"179A"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Output = "29184",
                    RawInput =
@"456A"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Output = "24256",
                    RawInput =
@"379A"
                },
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

        private readonly char AButton = 'A';

        private readonly string[] NumberKeypad = ["789", "456", "123", " 0A"];
        private readonly string[] DirectionalKeypad = [" ^A", "<v>"];
        private Dictionary<KeypadType, Base.Grid2Char> Keypads;
        private Dictionary<KeypadType, Base.Vec2> KeypadsAButton;
        private record FromTo(KeypadType Type, char From, char To);
        private Dictionary<FromTo, string> InstructionCache;

        private void ParseKeypads()
        {
            Keypads = [];
            KeypadsAButton = [];
            InstructionCache = [];

            Keypads[KeypadType.Number] = new(NumberKeypad);
            foreach (Base.Vec2 vec2 in Keypads[KeypadType.Number])
            {
                if (Keypads[KeypadType.Number][vec2] == AButton)
                {
                    KeypadsAButton[KeypadType.Number] = vec2;
                    break;
                }
            }

            Keypads[KeypadType.Directional] = new(DirectionalKeypad);
            foreach (Base.Vec2 vec2 in Keypads[KeypadType.Directional])
            {
                if (Keypads[KeypadType.Directional][vec2] == AButton)
                {
                    KeypadsAButton[KeypadType.Directional] = vec2;
                    break;
                }
            }
        }

        private string GetInputRequired(FromTo fromTo)
        {
            StringBuilder sb = new();
            if (fromTo.From == fromTo.To)
            {
                sb.Append(AButton);
                return sb.ToString();
            }

            if (InstructionCache.TryGetValue(fromTo, out string value))
            {
                return value;
            }

            Base.Grid2Char grid = Keypads[fromTo.Type];
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
                    sb = new(history[end]);
                    sb.Append(AButton);
                    return sb.ToString();
                }

                foreach (Util.Grid2.Dir dir in Util.Grid2.Iter.Cardinal)
                {
                    Base.Vec2 next = cur + Util.Grid2.Map.Neighbor[dir];
                    if (!grid.Contains(next) || history.ContainsKey(next) || grid[next] == ' ')
                    {
                        continue;
                    }

                    sb = new(history[cur]);
                    sb.Append(Util.Grid2.Map.SimpleArrow[dir]);
                    history[next] = sb.ToString();
                    queue.Enqueue(next);
                }
            }

            return string.Empty;
        }

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool _)
        {
            ParseKeypads();

            KeypadType[] process = [KeypadType.Number, KeypadType.Directional, KeypadType.Directional];

            StringBuilder sb;
            List<string> instructions = [.. inputs];
            foreach (KeypadType keypadType in process)
            {
                Log($"Processing: {keypadType}");
                List<string> curInstructions = [.. instructions];
                instructions.Clear();
                foreach (string curInstruction in curInstructions)
                {
                    sb = new();
                    char curButton = AButton;
                    foreach (char nextButton in curInstruction)
                    {
                        FromTo ft = new(keypadType, curButton, nextButton);
                        string inputRequired = GetInputRequired(ft);
                        // Log($"{ft.From} -> {ft.To} | {inputRequired}");
                        sb.Append(inputRequired);
                        curButton = nextButton;
                    }
                    Log($"{curInstruction} -> {sb}");
                    instructions.Add(sb.ToString());
                }
            }

            int sum = 0;
            for (int i = 0; i < inputs.Count; ++i)
            {
                int code = int.Parse(inputs[i][0..^1]);
                sum += code * instructions[i].Length;
                Log($"{instructions[i]} = {instructions[i].Length} * {code}");
            }

            return sum.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}