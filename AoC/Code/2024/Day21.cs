using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
//                 new Core.TestDatum
//                 {
//                     TestPart = Core.Part.One,
//                     Output = "1972",
//                     RawInput =
// @"029A"
//                 },
//                 new Core.TestDatum
//                 {
//                     TestPart = Core.Part.One,
//                     Output = "58800",
//                     RawInput =
// @"980A"
//                 },
//                 new Core.TestDatum
//                 {
//                     TestPart = Core.Part.One,
//                     Output = "12172",
//                     RawInput =
// @"179A"
//                 },
//                 new Core.TestDatum
//                 {
//                     TestPart = Core.Part.One,
//                     Output = "29184",
//                     RawInput =
// @"456A"
//                 },
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
        private record FromTo(KeypadType Type, char From, char To)
        {
            public FromTo Flip()
            {
                return new FromTo(Type, To, From);
            }
        }
        private Dictionary<FromTo, List<string>> InstructionCache;

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

            char n = Util.Grid2.Map.SimpleArrow[Util.Grid2.Dir.North];
            char e = Util.Grid2.Map.SimpleArrow[Util.Grid2.Dir.East];
            char s = Util.Grid2.Map.SimpleArrow[Util.Grid2.Dir.South];
            char w = Util.Grid2.Map.SimpleArrow[Util.Grid2.Dir.West];
            void addInstructions(KeypadType keypadType, char from, char to, List<List<Util.Grid2.Dir>> dirs)
            {
                List<string> instructions = [];
                foreach (List<Util.Grid2.Dir> dir in dirs)
                {
                    StringBuilder sb = new();
                    foreach (Util.Grid2.Dir d in dir)
                    {
                        sb.Append(Util.Grid2.Map.SimpleArrow[d]);
                    }
                    sb.Append(AButton);
                    instructions.Add(sb.ToString());
                }

                // to add the other one, use reverse dirs and use SimpleArrowFlipped
                FromTo ft = new(keypadType, from, to);
                InstructionCache[ft] = [.. instructions];

                instructions.Clear();
                foreach (List<Util.Grid2.Dir> dir in dirs)
                {
                    StringBuilder sb = new();
                    for (int i = dir.Count - 1; i >= 0; --i)
                    {
                        sb.Append(Util.Grid2.Map.SimpleArrow[Util.Grid2.Map.Opposite[dir[i]]]);
                    }
                    sb.Append(AButton);
                    instructions.Add(sb.ToString());
                }

                ft = ft.Flip();
                InstructionCache[ft] = instructions;
            }
            // TODO: Make this dynamic
            // A
            addInstructions(KeypadType.Number, 'A', '0', [[Util.Grid2.Dir.West]]);
            addInstructions(KeypadType.Number, 'A', '1', [[Util.Grid2.Dir.North, Util.Grid2.Dir.West, Util.Grid2.Dir.West]]);
            addInstructions(KeypadType.Number, 'A', '2', [[Util.Grid2.Dir.North, Util.Grid2.Dir.West],
                                                          [Util.Grid2.Dir.West, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, 'A', '3', [[Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, 'A', '4', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.West, Util.Grid2.Dir.West]]);
            addInstructions(KeypadType.Number, 'A', '5', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.West],
                                                          [Util.Grid2.Dir.West, Util.Grid2.Dir.North, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, 'A', '6', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, 'A', '7', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.West, Util.Grid2.Dir.West]]);
            addInstructions(KeypadType.Number, 'A', '8', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.West],
                                                          [Util.Grid2.Dir.West, Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, 'A', '9', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.North]]);
            // 0
            addInstructions(KeypadType.Number, '0', '1', [[Util.Grid2.Dir.North, Util.Grid2.Dir.West]]);
            addInstructions(KeypadType.Number, '0', '2', [[Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '0', '3', [[Util.Grid2.Dir.North, Util.Grid2.Dir.East],
                                                          [Util.Grid2.Dir.East, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '0', '4', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.West]]);
            addInstructions(KeypadType.Number, '0', '5', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '0', '6', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.East],
                                                          [Util.Grid2.Dir.East, Util.Grid2.Dir.North, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '0', '7', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.West]]);
            addInstructions(KeypadType.Number, '0', '8', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '0', '9', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.East],
                                                          [Util.Grid2.Dir.East, Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.North]]);
            // 1
            addInstructions(KeypadType.Number, '1', '2', [[Util.Grid2.Dir.East]]);
            addInstructions(KeypadType.Number, '1', '3', [[Util.Grid2.Dir.East, Util.Grid2.Dir.East]]);
            addInstructions(KeypadType.Number, '1', '4', [[Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '1', '5', [[Util.Grid2.Dir.North, Util.Grid2.Dir.East],
                                                          [Util.Grid2.Dir.East, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '1', '6', [[Util.Grid2.Dir.North, Util.Grid2.Dir.East, Util.Grid2.Dir.East],
                                                          [Util.Grid2.Dir.East, Util.Grid2.Dir.East, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '1', '7', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '1', '8', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.East],
                                                          [Util.Grid2.Dir.East, Util.Grid2.Dir.North, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '1', '9', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.East, Util.Grid2.Dir.East],
                                                          [Util.Grid2.Dir.East, Util.Grid2.Dir.East, Util.Grid2.Dir.North, Util.Grid2.Dir.North]]);
            // 2
            addInstructions(KeypadType.Number, '2', '3', [[Util.Grid2.Dir.East]]);
            addInstructions(KeypadType.Number, '2', '4', [[Util.Grid2.Dir.North, Util.Grid2.Dir.East],
                                                          [Util.Grid2.Dir.East, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '2', '5', [[Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '2', '6', [[Util.Grid2.Dir.North, Util.Grid2.Dir.East],
                                                          [Util.Grid2.Dir.East, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '2', '7', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.West],
                                                          [Util.Grid2.Dir.West, Util.Grid2.Dir.North, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '2', '8', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '2', '9', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.East],
                                                          [Util.Grid2.Dir.East, Util.Grid2.Dir.North, Util.Grid2.Dir.North]]);
            // 3
            addInstructions(KeypadType.Number, '3', '4', [[Util.Grid2.Dir.North, Util.Grid2.Dir.West, Util.Grid2.Dir.West],
                                                          [Util.Grid2.Dir.West, Util.Grid2.Dir.West, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '3', '5', [[Util.Grid2.Dir.North, Util.Grid2.Dir.West],
                                                          [Util.Grid2.Dir.West, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '3', '6', [[Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '3', '7', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.West, Util.Grid2.Dir.West],
                                                          [Util.Grid2.Dir.West, Util.Grid2.Dir.West, Util.Grid2.Dir.North, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '3', '8', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North, Util.Grid2.Dir.West],
                                                          [Util.Grid2.Dir.West, Util.Grid2.Dir.North, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '3', '9', [[Util.Grid2.Dir.North, Util.Grid2.Dir.North]]);
            // 4
            addInstructions(KeypadType.Number, '4', '5', [[Util.Grid2.Dir.East]]);
            addInstructions(KeypadType.Number, '4', '6', [[Util.Grid2.Dir.East, Util.Grid2.Dir.East]]);
            addInstructions(KeypadType.Number, '4', '7', [[Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '4', '8', [[Util.Grid2.Dir.North, Util.Grid2.Dir.East],
                                                          [Util.Grid2.Dir.East, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '4', '9', [[Util.Grid2.Dir.North, Util.Grid2.Dir.East, Util.Grid2.Dir.East],
                                                          [Util.Grid2.Dir.East, Util.Grid2.Dir.East, Util.Grid2.Dir.North]]);
            // 5
            addInstructions(KeypadType.Number, '5', '6', [[Util.Grid2.Dir.East]]);
            addInstructions(KeypadType.Number, '5', '7', [[Util.Grid2.Dir.North, Util.Grid2.Dir.West],
                                                          [Util.Grid2.Dir.West, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '5', '8', [[Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '5', '9', [[Util.Grid2.Dir.North, Util.Grid2.Dir.East],
                                                          [Util.Grid2.Dir.East, Util.Grid2.Dir.North]]);
            // 6
            addInstructions(KeypadType.Number, '6', '7', [[Util.Grid2.Dir.North, Util.Grid2.Dir.West, Util.Grid2.Dir.West],
                                                          [Util.Grid2.Dir.West, Util.Grid2.Dir.West, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '6', '8', [[Util.Grid2.Dir.North, Util.Grid2.Dir.West],
                                                          [Util.Grid2.Dir.West, Util.Grid2.Dir.North]]);
            addInstructions(KeypadType.Number, '6', '9', [[Util.Grid2.Dir.North]]);
            // 7
            addInstructions(KeypadType.Number, '7', '8', [[Util.Grid2.Dir.East]]);
            addInstructions(KeypadType.Number, '7', '9', [[Util.Grid2.Dir.East, Util.Grid2.Dir.East]]);
            // 8
            addInstructions(KeypadType.Number, '8', '9', [[Util.Grid2.Dir.East]]);
            // 9

            // TODO: Add KeyPadType.Direction instructions
            // A
            addInstructions(KeypadType.Directional, 'A', '^', [[Util.Grid2.Dir.West]]);
            addInstructions(KeypadType.Directional, 'A', '<', [[Util.Grid2.Dir.South, Util.Grid2.Dir.West, Util.Grid2.Dir.West]]);
            addInstructions(KeypadType.Directional, 'A', 'v', [[Util.Grid2.Dir.South, Util.Grid2.Dir.West],
                                                          [Util.Grid2.Dir.West, Util.Grid2.Dir.South]]);
            addInstructions(KeypadType.Directional, 'A', '>', [[Util.Grid2.Dir.South]]);
            // ^
            addInstructions(KeypadType.Directional, '^', '<', [[Util.Grid2.Dir.South, Util.Grid2.Dir.West]]);
            addInstructions(KeypadType.Directional, '^', 'v', [[Util.Grid2.Dir.South]]);
            addInstructions(KeypadType.Directional, '^', '>', [[Util.Grid2.Dir.South, Util.Grid2.Dir.East],
                                                          [Util.Grid2.Dir.East, Util.Grid2.Dir.South]]);
            // <
            addInstructions(KeypadType.Directional, '<', 'v', [[Util.Grid2.Dir.East]]);
            addInstructions(KeypadType.Directional, '<', '>', [[Util.Grid2.Dir.East, Util.Grid2.Dir.East]]);
            // v
            addInstructions(KeypadType.Directional, 'v', '>', [[Util.Grid2.Dir.East]]);
            // >
        }

        private List<string> GetInputRequired(FromTo fromTo)
        {
            StringBuilder sb = new();
            if (fromTo.From == fromTo.To)
            {
                sb.Append(AButton);
                return new([sb.ToString()]);
            }

            if (InstructionCache.TryGetValue(fromTo, out List<string> value))
            {
                return value;
            }

            return [];
        }

        private Dictionary<long, Dictionary<FromTo, long>> ShortestPath;
        private Dictionary<long, Dictionary<FromTo, string>> ShortestVisualPath;

        private long GetShortestPath(FromTo ft, int level, int maxLevel)
        {
            Dictionary<FromTo, string> visualPaths = [];
            if (!ShortestPath.TryGetValue(level, out Dictionary<FromTo, long> levelPaths))
            {
                ShortestPath[level] = [];
                levelPaths = ShortestPath[level];

                ShortestVisualPath[level] = visualPaths;
            }
            else
            {
                visualPaths = ShortestVisualPath[level];
            }

            if (levelPaths.TryGetValue(ft, out long value))
            {
                return value;
            }

            if (ft.From == ft.To)
            {
                return 1;
            }

            // Log($"[{level}] Processing: {new string(' ', level * 2)} {ft.From}{ft.To}");

            // StringBuilder sb;
            long shortestPath = long.MaxValue;
            foreach (string instruction in InstructionCache[ft])
            {
                // Log($"[{level}] Processing:   {new string(' ', level * 2)} {instruction}");
                long preShortestPath = shortestPath;
                if (level >= maxLevel)
                {
                    shortestPath = Math.Min(shortestPath, instruction.Length);
                }
                else
                {
                    long curPath = 0;
                    char curButton = AButton;
                    foreach (char nextButton in instruction)
                    {
                        FromTo subFT = new(KeypadType.Directional, curButton, nextButton);
                        curPath += GetShortestPath(subFT, level + 1, maxLevel);
                        curButton = nextButton;
                    }
                    shortestPath = Math.Min(shortestPath, curPath);
                }
                if (shortestPath < preShortestPath)
                {
                    visualPaths[ft] = instruction;
                }
            }

            // Log($"[{level}] ShortestPath: {new string(' ', level * 2)} {visualPaths[ft]}");
            levelPaths[ft] = shortestPath;
            return shortestPath;
        }

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, int directionalCount)
        {
            ParseKeypads();
            ShortestPath = [];
            ShortestVisualPath = [];

            // KeypadType[] process = new KeypadType[directionalCount + 1];
            // process[0] = KeypadType.Number;
            // for (int i = 1; i <= directionalCount; ++i)
            // {
            //     process[i] = KeypadType.Directional;
            // }

            StringBuilder sb;
            List<string> instructions = [.. inputs];
            List<long> instructionLengths = [];
            int _i = 0;
            //foreach (KeypadType keypadType in process)
            {
                //Log($"[{_i++}] Processing: {keypadType}");
                List<string> curInstructions = [.. instructions];
                instructions.Clear();
                foreach (string curInstruction in curInstructions)
                {
                    Log($"[{_i}] Processing: {curInstruction}");
                    instructionLengths.Add(0);
                    sb = new();
                    char curButton = AButton;
                    foreach (char nextButton in curInstruction)
                    {
                        FromTo ft = new(KeypadType.Number, curButton, nextButton);
                        instructionLengths[_i] += GetShortestPath(ft, 0, directionalCount);
                        curButton = nextButton;
                    }
                    
                    ++_i;
                }
            }

            long sum = 0;
            for (int i = 0; i < inputs.Count; ++i)
            {
                long code = long.Parse(inputs[i][0..^1]);
                sum += code * instructionLengths[i];
                Log($"{inputs[i]} = {instructionLengths[i]} * {code}");
            }

            return sum.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, 2);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, 25);
    }
}