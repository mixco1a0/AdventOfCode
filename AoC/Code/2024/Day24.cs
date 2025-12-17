using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoC.Util;

namespace AoC._2024
{
    class Day24 : Core.Day
    {
        public Day24() { }

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
                    Output = "4",
                    RawInput =
@"x00: 1
x01: 1
x02: 1
y00: 0
y01: 1
y02: 0

x00 AND y00 -> z00
x01 XOR y01 -> z01
x02 OR y02 -> z02"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Output = "2024",
                    RawInput =
@"x00: 1
x01: 0
x02: 1
x03: 1
x04: 0
y00: 1
y01: 1
y02: 1
y03: 1
y04: 1

ntg XOR fgs -> mjb
y02 OR x01 -> tnw
kwq OR kpj -> z05
x00 OR x03 -> fst
tgd XOR rvg -> z01
vdt OR tnw -> bfw
bfw AND frj -> z10
ffh OR nrd -> bqk
y00 AND y03 -> djm
y03 OR y00 -> psh
bqk OR frj -> z08
tnw OR fst -> frj
gnj AND tgd -> z11
bfw XOR mjb -> z00
x03 OR x00 -> vdt
gnj AND wpb -> z02
x04 AND y00 -> kjc
djm OR pbm -> qhw
nrd AND vdt -> hwm
kjc AND fst -> rvg
y04 OR y02 -> fgs
y01 AND x02 -> pbm
ntg OR kjc -> kwq
psh XOR fgs -> tgd
qhw XOR tgd -> z09
pbm OR djm -> kpj
x03 XOR y03 -> ffh
x00 XOR y04 -> ntg
bfw OR bqk -> z06
nrd XOR fgs -> wpb
frj XOR qhw -> z04
bqk OR frj -> z07
y03 OR x01 -> nrd
hwm AND bqk -> z03
tgd XOR rvg -> z12
tnw OR pbm -> gnj"
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

        static uint ConvertName(string name)
        {
            return (uint)name[0] << 16 | (uint)name[1] << 8 | (uint)name[2];
        }

        static string ConvertName(uint i)
        {
            StringBuilder sb = new();
            sb.Append((char)((0xff0000 & i) >> 16));
            sb.Append((char)((0xff00 & i) >> 8));
            sb.Append((char)(0xff & i));
            return sb.ToString();
        }

        private enum Op
        {
            Invalid,
            And,
            Or,
            Xor
        }

        private record Gate(uint InputA, uint InputB, uint Output, Op Op)
        {
            public UInt128 GetId()
            {
                return (UInt128)InputA << 72 | (UInt128)InputB << 40 | (UInt128)Output << 8 | (ulong)Op;
            }

            public static Gate Parse(string input)
            {
                string[] split = Util.String.Split(input, " ->");
                uint inputA = ConvertName(split[0]);
                uint inputB = ConvertName(split[2]);
                uint output = ConvertName(split[3]);
                Op op = Op.Invalid;
                switch (split[1][0])
                {
                    case 'A':
                        op = Op.And;
                        break;
                    case 'O':
                        op = Op.Or;
                        break;
                    case 'X':
                        op = Op.Xor;
                        break;
                }
                return new(inputA, inputB, output, op);
            }

            public override string ToString()
            {
                return $"{ConvertName(InputA)} {Op} {ConvertName(InputB)} -> {ConvertName(Output)}";
            }
        }

        private void Parse(List<string> inputs, out Dictionary<uint, bool> memory, out List<Gate> gates)
        {
            memory = [];
            gates = [];

            bool parseWire = true;
            foreach (string input in inputs)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    parseWire = false;
                    continue;
                }

                if (parseWire)
                {
                    string[] split = Util.String.Split(input, ": ");
                    uint wire = ConvertName(split[0]);
                    memory[wire] = split[1][0] == '1';
                }
                else
                {
                    gates.Add(Gate.Parse(input));
                }
            }
        }

        private Dictionary<uint, bool> GenerateZ(Dictionary<uint, bool> memory, List<Gate> gates)
        {
            List<Gate> temp = [.. gates];
            while (temp.Count > 0)
            {
                List<Gate> pending = [.. temp];
                foreach (Gate gate in pending)
                {
                    if (memory.TryGetValue(gate.InputA, out bool inputA) && memory.TryGetValue(gate.InputB, out bool inputB))
                    {
                        switch (gate.Op)
                        {
                            case Op.And:
                                memory[gate.Output] = inputA && inputB;
                                break;
                            case Op.Or:
                                memory[gate.Output] = inputA || inputB;
                                break;
                            case Op.Xor:
                                memory[gate.Output] = inputA != inputB;
                                break;
                        }
                        // Log($"{ConvertName(gate.InputA)} {gate.Op} {ConvertName(gate.InputB)} -> {gate.Output} = {memory[gate.Output]}");
                        temp.Remove(gate);
                        break;
                    }
                }
            }
            return memory;
        }

        private ulong GetNumber(Dictionary<uint, bool> memory, char variable, out int bitCount)
        {
            List<bool> bits = [.. memory.Where(m => (char)((m.Key & 0xff0000) >> 16) == variable).OrderByDescending(m => m.Key).Select(m => m.Value)];
            bitCount = 1;
            ulong value = (ulong)(bits[0] ? 1 : 0);
            foreach (bool bit in bits.Skip(1))
            {
                ++bitCount;
                value <<= 1;
                if (bit)
                {
                    value |= 1;
                }
            }
            return value;
        }

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool fixGates)
        {
            Parse(inputs, out Dictionary<uint, bool> initialMemory, out List<Gate> initialGates);

            Dictionary<uint, bool> memory = GenerateZ(initialMemory, initialGates);
            ulong zValue = GetNumber(memory, 'z', out int bitCount);
            if (!fixGates)
            {
                return zValue.ToString();
            }

            ulong xValue = GetNumber(memory, 'x', out _);
            ulong yValue = GetNumber(memory, 'y', out _);
            
            HashSet<UInt128> safeGateIds = [];
            List<Gate> safeGates = [];
            Dictionary<uint, bool> safeMemory = [];

            ulong zActual = xValue + yValue;
            for (int i = 0; i < bitCount; ++i)
            {
                // get current z node
                string zNode = $"z{i:D2}";
                uint zNodeId = ConvertName(zNode);

                // work backwords to find entire
                HashSet<uint> usedNodes = [];
                List<Gate> curNodeGates = [];
                Queue<uint> nodesToProcess = [];
                nodesToProcess.Enqueue(zNodeId);
                while (nodesToProcess.Count > 0)
                {
                    uint curNodeId = nodesToProcess.Dequeue();
                    if (usedNodes.Contains(curNodeId))
                    {
                        continue;
                    }
                    usedNodes.Add(curNodeId);

                    foreach (Gate g in initialGates.Where(g => g.Output == curNodeId && !safeGateIds.Contains(g.GetId())))
                    {
                        curNodeGates.Add(g);
                        // Log($"Adding {g}");
                    }

                    foreach (uint nodeId in initialGates.Where(g => g.Output == curNodeId && !safeGateIds.Contains(g.GetId())).SelectMany<Gate, uint>(g => [g.InputA, g.InputB]))
                    {
                        nodesToProcess.Enqueue(nodeId);
                        // Log($"Process {ConvertName(nodeId)}");
                    }
                }

                ulong flag = (ulong)(1 << i);
                ulong x = xValue & flag;
                ulong y = yValue & flag;
                ulong z = zValue & flag;
                ulong _z = zActual & flag;
                
                Log($"Processing {zNode}...");

                if (z == _z)
                {
                    Log($"...safe");
                    // loop through all nodes used for this and add them to the safe gates
                    foreach (Gate g in curNodeGates)
                    {
                        Log($"......{g}");
                        safeGateIds.Add(g.GetId());
                        safeGates.Add(g);
                    }

                    foreach (uint node in usedNodes)
                    {
                        safeMemory[node] = memory[node];
                    }
                }
                else
                {
                    Log($"...unsafe");
                    // loop through all nodes used for this and add them to the safe gates
                    foreach (Gate g in curNodeGates)
                    {
                        string inputA = ConvertName(g.InputA);
                        if (safeMemory.TryGetValue(g.InputA, out bool valueA))
                        {
                            inputA = valueA ? "1" : "0";
                        }
                        else
                        {
                            char letter = (char)((g.InputA & 0xff0000) >> 16);
                            if (letter == 'x' || letter == 'y')
                            {
                                inputA = memory[g.InputA] ? "1" : "0";
                            }
                        }

                        string inputB= ConvertName(g.InputB);
                        if (safeMemory.TryGetValue(g.InputB, out bool valueB))
                        {
                            inputB = valueB ? "1" : "0";
                        }
                        else
                        {
                            char letter = (char)((g.InputB & 0xff0000) >> 16);
                            if (letter == 'x' || letter == 'y')
                            {
                                inputB = memory[g.InputB] ? "1" : "0";
                            }
                        }

                        Log($"......{inputA} {g.Op} {inputB} -> {ConvertName(g.Output)} | {g}");
                        // safeGateIds.Add(g.GetId());
                        // safeGates.Add(g);
                    }

                    // Log("."); Log("..");
                    // Log($"Node {zNode} is wrong");

                    // foreach (Gate gate in curNodeGates)
                    // {
                    //     Log($"{ConvertName(gate.InputA)} {gate.Op} {ConvertName(gate.InputB)} -> {ConvertName(gate.Output)} = {memory[gate.Output]}");
                    // }
                }
            }

            Log($"  0x{xValue:B}");
            Log($"+ 0x{yValue:B}");
            Log($"----{new string('-', bitCount)}--");
            Log($" 0x{(xValue + yValue):B}");
            Log($"----{new string('-', bitCount)}--");
            Log($" 0x{zValue:B}");
            return "";
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}