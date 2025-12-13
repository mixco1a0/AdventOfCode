using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool _)
        {
            Parse(inputs, out Dictionary<uint, bool> memory, out List<Gate> gates);

            while (gates.Count > 0)
            {
                List<Gate> pending = [.. gates];
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
                        gates.Remove(gate);
                        break;
                    }
                }
            }

            List<bool> zBits = [.. memory.Where(m => (char)((m.Key & 0xff0000) >> 16) == 'z').OrderByDescending(m => m.Key).Select(m => m.Value)];
            ulong zValue = (ulong)(zBits[0] ? 1 : 0);
            foreach (bool zBit in zBits.Skip(1))
            {
                zValue <<= 1;
                if (zBit)
                {
                    zValue |= 1;
                }
            }

            return zValue.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}