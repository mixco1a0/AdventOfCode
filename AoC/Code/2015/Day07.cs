using System.Collections.Generic;
using System.Linq;

namespace AoC._2015
{
    class Day07 : Core.Day
    {
        public Day07() { }

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
                    Variables = new Dictionary<string, string> { { nameof(_Wire), "d" } },
                    Output = "72",
                    RawInput =
@"123 -> x
456 -> y
x AND y -> d
x OR y -> e
x LSHIFT 2 -> f
y RSHIFT 2 -> g
NOT x -> h
NOT y -> i"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Variables = new Dictionary<string, string> { { nameof(_Wire), "e" } },
                    Output = "507",
                    RawInput =
@"123 -> x
456 -> y
x AND y -> d
x OR y -> e
x LSHIFT 2 -> f
y RSHIFT 2 -> g
NOT x -> h
NOT y -> i"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Variables = new Dictionary<string, string> { { nameof(_Wire), "h" } },
                    Output = "65412",
                    RawInput =
@"123 -> x
456 -> y
x AND y -> d
x OR y -> e
x LSHIFT 2 -> f
y RSHIFT 2 -> g
NOT x -> h
NOT y -> i"
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
        private static string _Wire { get; }
#pragma warning restore IDE1006 // Naming Styles

        private const char InvalidSignal = '_';

        private class Instruction
        {
            public enum InstructionType
            {
                Invalid,
                Set,
                And,
                Or,
                LShift,
                RShift,
                Not,
                SetRef,
                AndRef,
                OrRef,
            }

            public InstructionType Type { get; private set; }
            public string Signal1 { get; private set; }
            public string Signal2 { get; private set; }
            public int Value { get; private set; }
            public string Destination { get; private set; }
            public bool Complete { get; private set; }

            public Instruction()
            {
                Type = InstructionType.Invalid;
                Signal1 = string.Empty;
                Signal2 = string.Empty;
                Value = 0;
                Destination = string.Empty;
                Complete = false;
            }

            public Instruction(Instruction other)
            {
                Type = other.Type;
                Signal1 = other.Signal1;
                Signal2 = other.Signal2;
                Value = other.Value;
                Destination = other.Destination;
                Complete = false;
            }

            public static Instruction Parse(string input)
            {
                Instruction instruction = new()
                {
                    Signal2 = string.Empty
                };
                string[] split = [.. input.Split(" ")];
                int[] intVals = [.. split.Where(s => { return int.TryParse(s, out int i); }).Select(int.Parse)];
                instruction.Value = intVals.FirstOrDefault();
                if (input.Contains("AND"))
                {
                    if (intVals.Length > 0)
                    {
                        instruction.Type = InstructionType.And;
                    }
                    else
                    {
                        instruction.Type = InstructionType.AndRef;
                    }
                }
                else if (input.Contains("OR"))
                {
                    if (intVals.Length > 0)
                    {
                        instruction.Type = InstructionType.Or;
                    }
                    else
                    {
                        instruction.Type = InstructionType.OrRef;
                    }
                }
                else if (input.Contains("LSHIFT"))
                {
                    instruction.Type = InstructionType.LShift;
                }
                else if (input.Contains("RSHIFT"))
                {
                    instruction.Type = InstructionType.RShift;
                }
                else if (input.Contains("NOT"))
                {
                    instruction.Type = InstructionType.Not;
                }
                else
                {
                    if (intVals.Length > 0)
                    {
                        instruction.Type = InstructionType.Set;
                    }
                    else
                    {
                        instruction.Type = InstructionType.SetRef;
                    }
                }

                switch (instruction.Type)
                {
                    case InstructionType.AndRef:
                    case InstructionType.OrRef:
                    case InstructionType.LShift:
                    case InstructionType.RShift:
                        instruction.Signal1 = split[0];
                        instruction.Signal2 = split[2];
                        instruction.Destination = split[4];
                        break;
                    case InstructionType.And:
                    case InstructionType.Or:
                        if (int.TryParse(split[0], out int test))
                        {
                            instruction.Signal1 = split[2];
                            instruction.Signal2 = split[0];
                            instruction.Destination = split[4];
                        }
                        else
                        {
                            instruction.Signal1 = split[0];
                            instruction.Signal2 = split[2];
                            instruction.Destination = split[4];
                        }
                        break;
                    case InstructionType.Not:
                        instruction.Signal1 = split[1];
                        instruction.Destination = split[3];
                        break;
                    case InstructionType.Set:
                    case InstructionType.SetRef:
                        instruction.Signal1 = split[0];
                        instruction.Destination = split[2];
                        break;
                }

                return instruction;
            }

            public bool CanExecute(Dictionary<string, int> signals)
            {
                switch (Type)
                {
                    case InstructionType.AndRef:
                    case InstructionType.OrRef:
                        if (signals.ContainsKey(Signal1) && signals.ContainsKey(Signal2))
                        {
                            return !Complete;
                        }
                        break;
                    case InstructionType.And:
                    case InstructionType.Or:
                    case InstructionType.LShift:
                    case InstructionType.RShift:
                    case InstructionType.Not:
                    case InstructionType.SetRef:
                        if (signals.ContainsKey(Signal1))
                        {
                            return !Complete;
                        }
                        break;
                    case InstructionType.Set:
                        return !Complete;
                }
                return false;
            }

            public void Execute(ref Dictionary<string, int> signals)
            {
                switch (Type)
                {
                    case InstructionType.And:
                        signals[Destination] = signals[Signal1] & Value;
                        break;
                    case InstructionType.AndRef:
                        signals[Destination] = signals[Signal1] & signals[Signal2];
                        break;
                    case InstructionType.Or:
                        signals[Destination] = signals[Signal1] | Value;
                        break;
                    case InstructionType.OrRef:
                        signals[Destination] = signals[Signal1] | signals[Signal2];
                        break;
                    case InstructionType.LShift:
                        signals[Destination] = signals[Signal1] << Value;
                        break;
                    case InstructionType.RShift:
                        signals[Destination] = signals[Signal1] >> Value;
                        break;
                    case InstructionType.Not:
                        signals[Destination] = ~signals[Signal1];
                        break;
                    case InstructionType.Set:
                        signals[Destination] = Value;
                        Complete = true;
                        break;
                    case InstructionType.SetRef:
                        signals[Destination] = signals[Signal1];
                        Complete = true;
                        break;
                }
                signals[Destination] &= ushort.MaxValue;
                Complete = true;
            }

            public override string ToString()
            {
                return Type switch
                {
                    InstructionType.And or InstructionType.AndRef or InstructionType.Or or InstructionType.OrRef or InstructionType.LShift or InstructionType.RShift => $"{Signal1} {Type} {Signal2} -> {Destination}",
                    InstructionType.Not => $"{Type} {Signal1} -> {Destination}",
                    InstructionType.Set or InstructionType.SetRef => $"{Signal1} -> {Destination}",
                    _ => base.ToString(),
                };
            }
        }

        private static Dictionary<string, int> RunToCompletion(List<Instruction> instructions)
        {
            Dictionary<string, int> signals = [];
            Queue<Instruction> pending = new(instructions);
            while (pending.Count > 0)
            {
                Instruction cur = pending.Dequeue();
                if (cur.CanExecute(signals))
                {
                    cur.Execute(ref signals);
                }
                else
                {
                    pending.Enqueue(cur);
                }
            }

            return signals;
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, char signalReset)
        {
            GetVariable(nameof(_Wire), "a", variables, out string wire);

            List<Instruction> instructions = [.. inputs.Select(Instruction.Parse)];
            instructions.Sort((a, b) => a.Type != b.Type ? (a.Type > b.Type ? 1 : -1) : a.Destination.CompareTo(b.Destination));
            Dictionary<string, int> signals = RunToCompletion(instructions);

            if (signalReset != InvalidSignal)
            {
                string prevSignal = signals[wire].ToString();
                string signalResetString = $"{signalReset}";
                instructions = [.. instructions.Where(i => i.Destination != signalResetString).Select(i => new Instruction(i))];
                instructions.Insert(0, Instruction.Parse($"{prevSignal} -> {signalReset}"));
                signals = RunToCompletion(instructions);
            }

            return signals[wire].ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, InvalidSignal);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, 'b');
    }
}