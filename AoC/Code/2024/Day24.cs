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
                return $"{ConvertName(InputA)} {Op,-3} {ConvertName(InputB)} -> {ConvertName(Output)}";
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
                int pendingCount = pending.Count;
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

                if (pendingCount == temp.Count)
                {
                    memory = [];
                    break;
                }
            }
            return memory;
        }

        private ulong GetNumber(Dictionary<uint, bool> memory, char variable, out int maxBit)
        {
            List<bool> bits = [.. memory.Where(m => (char)((m.Key & 0xff0000) >> 16) == variable).OrderByDescending(m => m.Key).Select(m => m.Value)];
            maxBit = 1;
            ulong value = (ulong)(bits[0] ? 1 : 0);
            foreach (bool bit in bits.Skip(1))
            {
                ++maxBit;
                value <<= 1;
                if (bit)
                {
                    value |= 1;
                }
            }
            return value;
        }

        private void GetNodeInformation(List<Gate> gates, HashSet<UInt128> safeGateIds, uint zNodeId, out HashSet<uint> usedNodes, out List<Gate> curNodeGates)
        {
            usedNodes = [];
            curNodeGates = [];
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

                foreach (Gate g in gates.Where(g => g.Output == curNodeId && !safeGateIds.Contains(g.GetId())))
                {
                    curNodeGates.Add(g);
                }

                foreach (uint nodeId in gates.Where(g => g.Output == curNodeId && !safeGateIds.Contains(g.GetId())).SelectMany<Gate, uint>(g => [g.InputA, g.InputB]))
                {
                    nodesToProcess.Enqueue(nodeId);
                }
            }
        }

        private bool NodeHasCorrectGates(int curBit, int maxBit, List<Gate> curNodeGates)
        {
            int xorCount = curNodeGates.Count(g => g.Op == Op.Xor);
            int andCount = curNodeGates.Count(g => g.Op == Op.And);
            int orCount = curNodeGates.Count(g => g.Op == Op.Or);

            bool curIndexSafe = false;
            // if patterns match, send to safe, else, send to unsafe
            if (curBit == 0)
            {
                // pattern requires 1 XOR
                curIndexSafe = xorCount == 1 && andCount == 0 && orCount == 0;
            }
            else if (curBit == 1)
            {
                // pattern requires 1 AND and 2 XOR
                curIndexSafe = xorCount == 2 && andCount == 1 && orCount == 0;
            }
            else if (curBit == (maxBit - 1))
            {
                // nothing else to fix against
                curIndexSafe = true;
            }
            else
            {
                // pattern requires 2 AND, 2 XOR, and 1 OR
                curIndexSafe = xorCount == 2 && andCount == 2 && orCount == 1;
            }
            return curIndexSafe;
        }

        private bool RunBitTests(Dictionary<uint, bool> initialMemory, List<Gate> gates, int bit, int maxBit)
        {
            ulong allBits = (ulong)((1 << bit) - 1);

            // need to run through a suite of tests to make sure things are actually summed correctly
            List<Base.KeyVal<ulong, ulong>> xyValues =
            [
                new(0, 0),
                new(0, allBits),
                new(allBits, 0),
                new(allBits, allBits)
            ];

            foreach (var pair in xyValues)
            {
                ulong x = pair.First;
                ulong y = pair.Last;
                ulong z = x + y;

                // fill int the dynamic memory
                Dictionary<uint, bool> memory = [];
                for (int i = 0; i < bit; ++i)
                {
                    string xNode = $"x{i:D2}";
                    uint xNodeId = ConvertName(xNode);
                    string yNode = $"y{i:D2}";
                    uint yNodeId = ConvertName(yNode);

                    ulong flag = (ulong)(1 << i);
                    memory[xNodeId] = (x & flag) != 0;
                    memory[yNodeId] = (y & flag) != 0;
                }

                // pad the rest of the memory as empty
                for (int i = bit; i < (maxBit - 1); ++i)
                {
                    string xNode = $"x{i:D2}";
                    uint xNodeId = ConvertName(xNode);
                    string yNode = $"y{i:D2}";
                    uint yNodeId = ConvertName(yNode);
                    
                    memory[xNodeId] = false;
                    memory[yNodeId] = false;
                }

                // see if the bits were all correct in the output
                if (!RunBitTest(memory, gates, bit))
                {
                    return false;
                }

            }

            // if (!RunBitTest(initialMemory, gates, bit))
            // {
            //     return false;
            // }

            return true;
        }

        private bool RunBitTest(Dictionary<uint, bool> initialMemory, List<Gate> gates, int maxBit)
        {
            Dictionary<uint, bool> memory = GenerateZ(initialMemory, gates);

            // gates weren't able to fully complete
            if (memory.Count == 0)
            {
                return false;
            }

            // make sure the expected values were correct
            ulong zValue = GetNumber(memory, 'z', out int bitCount);
            ulong xValue = GetNumber(memory, 'x', out _);
            ulong yValue = GetNumber(memory, 'y', out _);
            ulong zActual = xValue + yValue;

            for (int i = 0; i <= maxBit; ++i)
            {
                ulong flag = (ulong)(1 << i);
                ulong x = xValue & flag;
                ulong y = yValue & flag;
                ulong z = zValue & flag;
                ulong _z = zActual & flag;

                if (z != _z)
                {
                    return false;
                }
            }

            return true;
        }

        private void TryAndFix(Dictionary<uint, bool> initialMemory, ref List<Gate> gates, HashSet<UInt128> safeGateIds, int bit, int maxBit, ref HashSet<uint> swapped)
        {
            string zCurNode = $"z{bit:D2}";
            uint zCurNodeId = ConvertName(zCurNode);
            GetNodeInformation(gates, safeGateIds, zCurNodeId, out HashSet<uint> _, out List<Gate> curNodeGates);

            string zNextNode = $"z{(bit + 1):D2}";
            uint zNextNodeId = ConvertName(zNextNode);
            GetNodeInformation(gates, safeGateIds, zNextNodeId, out HashSet<uint> _, out List<Gate> nextNodeGates);
            nextNodeGates.RemoveAll(g => curNodeGates.Contains(g));

            for (int i = 0; i < curNodeGates.Count; ++i)
            {
                Gate curNodeAsIs = curNodeGates[i];

                for (int j = 0; j < nextNodeGates.Count; ++j)
                {
                    Gate nextNodeAsIs = nextNodeGates[j];

                    // can they even be swapped?
                    if (curNodeAsIs.Output == nextNodeAsIs.InputA || curNodeAsIs.Output == nextNodeAsIs.InputB || nextNodeAsIs.Output == curNodeAsIs.InputA || nextNodeAsIs.Output == curNodeAsIs.InputB)
                    {
                        continue;
                    }

                    List<Gate> tempGates = [.. gates];

                    tempGates.Remove(curNodeAsIs);
                    tempGates.Remove(nextNodeAsIs);

                    Gate newCur = new(curNodeAsIs.InputA, curNodeAsIs.InputB, nextNodeAsIs.Output, curNodeAsIs.Op);
                    Gate newNext = new(nextNodeAsIs.InputA, nextNodeAsIs.InputB, curNodeAsIs.Output, nextNodeAsIs.Op);
                    tempGates.Add(newCur);
                    tempGates.Add(newNext);

                    // do the patterns match now?
                    GetNodeInformation(tempGates, safeGateIds, zCurNodeId, out HashSet<uint> _, out List<Gate> tempCurNodeGates);
                    GetNodeInformation(tempGates, safeGateIds, zNextNodeId, out HashSet<uint> _, out List<Gate> tempNextNodeGates);
                    tempNextNodeGates.RemoveAll(g => tempCurNodeGates.Contains(g));
                    if (!NodeHasCorrectGates(bit, maxBit, tempCurNodeGates) || !NodeHasCorrectGates(bit, maxBit, tempNextNodeGates))
                    {
                        continue;
                    }

                    // are the nodes actually functional now?
                    if (RunBitTests(initialMemory, tempGates, bit + 1, maxBit))
                    {
                        swapped.Add(curNodeAsIs.Output);
                        swapped.Add(nextNodeAsIs.Output);

                        Log($"Swapping...");
                        Log($"     {curNodeAsIs} | {newCur}");
                        Log($"     {nextNodeAsIs} | {newNext}");

                        gates = [.. tempGates];
                        break;
                    }
                }
            }
        }

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool fixGates)
        {
            Parse(inputs, out Dictionary<uint, bool> initialMemory, out List<Gate> initialGates);

            Dictionary<uint, bool> memory = GenerateZ(initialMemory, initialGates);
            ulong zValue = GetNumber(memory, 'z', out int maxBit);
            if (!fixGates)
            {
                return zValue.ToString();
            }

            List<Gate> workingGates = [.. initialGates];

            ulong xValue = GetNumber(memory, 'x', out _);
            ulong yValue = GetNumber(memory, 'y', out _);

            HashSet<uint> swapped = [];

            HashSet<UInt128> safeGateIds = [];
            List<Gate> safeGates = [];
            Dictionary<uint, bool> safeMemory = [];
            HashSet<UInt128> unsafeGateIds = [];
            List<Gate> unsafeGates = [];

            ulong zActual = xValue + yValue;
            for (int bit = 0; bit < maxBit; /*do nothing*/)
            {
                // get current z node
                string zNode = $"z{bit:D2}";
                uint zNodeId = ConvertName(zNode);

                // work backwords to find entire
                GetNodeInformation(workingGates, safeGateIds, zNodeId, out HashSet<uint> usedNodes, out List<Gate> curNodeGates);
                bool curIndexSafe = NodeHasCorrectGates(bit, maxBit, curNodeGates);

                ulong flag = (ulong)(1 << bit);
                ulong x = xValue & flag;
                ulong y = yValue & flag;
                ulong z = zValue & flag;
                ulong _z = zActual & flag;

                Log($"Processing {zNode}...");

                if (curIndexSafe)
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
                        Log($".........{ConvertName(node)} = {memory[node]}");
                    }

                    ++bit;
                }
                else
                {
                    Log($"...unsafe");
                    // loop through all nodes used for this and add them to the safe gates
                    foreach (Gate g in curNodeGates)
                    {
                        Log($"......{g}");
                    }

                    foreach (uint node in usedNodes)
                    {
                        Log($".........{ConvertName(node)} = {memory[node]}");
                    }
                    TryAndFix(initialMemory, ref workingGates, safeGateIds, bit, maxBit, ref swapped);
                    memory = GenerateZ(initialMemory, workingGates);
                }
            }

            Log($"  0x{xValue:B}");
            Log($"+ 0x{yValue:B}");
            Log($"----{new string('-', maxBit)}--");
            Log($" 0x{(xValue + yValue):B}");
            Log($"----{new string('-', maxBit)}--");

            safeMemory = GenerateZ(initialMemory, workingGates);
            ulong zFinalValue = GetNumber(memory, 'z', out int _);
            Log($" 0x{zFinalValue:B}");

            return string.Join(',', swapped.Select(output => ConvertName(output)).Order());
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}