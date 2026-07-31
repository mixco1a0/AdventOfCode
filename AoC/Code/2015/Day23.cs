using System;
using System.Collections.Generic;

namespace AoC._2015
{
    class Day23 : Core.Day
    {
        public Day23() { }

        public override string GetSolutionVersion(Core.Part part)
        {
            return part switch
            {
                Core.Part.One => "v1",
                Core.Part.Two => "v1",
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
                    Variables = new Dictionary<string, string> { { nameof(_ReturnRegister), "a" } },
                    Output = "2",
                    RawInput =
@"inc a
jio a, +2
tpl a
inc a"
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
        private int _ReturnRegister { get; }
#pragma warning restore IDE1006 // Naming Styles

        enum Instruction
        {
            Half,
            Triple,
            Increment,
            Jump,
            JumpIfEven,
            JumpIfOne
        }

        record InstructionSet(Instruction Instruction, char Register, int Offset) { }

        static List<InstructionSet> ParseInstructions(List<string> inputs)
        {
            List<InstructionSet> instructions = [];
            foreach (string input in inputs)
            {
                string[] parts = Util.String.Split(input, " ,");
                InstructionSet newSet = null;
                if (parts[0][0] == 'h')
                {
                    newSet = new InstructionSet(Instruction.Half, parts[1][0], 0);
                }
                else if (parts[0][0] == 't')
                {
                    newSet = new InstructionSet(Instruction.Triple, parts[1][0], 0);
                }
                else if (parts[0][0] == 'i')
                {
                    newSet = new InstructionSet(Instruction.Increment, parts[1][0], 1);
                }
                else if (parts[0] == "jmp")
                {
                    newSet = new InstructionSet(Instruction.Jump, ' ', int.Parse(parts[1]));
                }
                else if (parts[0] == "jie")
                {
                    newSet = new InstructionSet(Instruction.JumpIfEven, parts[1][0], int.Parse(parts[2]));
                }
                else if (parts[0] == "jio")
                {
                    newSet = new InstructionSet(Instruction.JumpIfOne, parts[1][0], int.Parse(parts[2]));
                }

                if (newSet != null)
                {
                    instructions.Add(newSet);
                }
            }
            return instructions;
        }

        static void PerformOn(Instruction instruction, int offset, ref uint register)
        {
            switch (instruction)
            {
                case Instruction.Half:
                    register /= 2;
                    break;
                case Instruction.Triple:
                    register *= 3;
                    break;
                case Instruction.Increment:
                    if (offset > 0)
                    {
                        register += (uint)offset;
                    }
                    else
                    {
                        register -= (uint)offset;
                    }
                    break;
            }
        }

        static void Perform(Instruction instruction, char register, int offset, ref uint a, ref uint b)
        {
            if (register == 'a')
            {
                PerformOn(instruction, offset, ref a);
            }
            else
            {
                PerformOn(instruction, offset, ref b);
            }
        }

        static bool IsEven(uint a, uint b, char register)
        {
            if (register == 'a')
            {
                return a % 2 == 0;
            }
            return b % 2 == 0;
        }

        static bool IsOne(uint a, uint b, char register)
        {
            if (register == 'a')
            {
                return a == 1;
            }
            return b == 1;
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, uint initialRegAVal)
        {
            GetVariable(nameof(_ReturnRegister), "b", variables, out string returnRegister);

            List<InstructionSet> instructions = ParseInstructions(inputs);
            int curInstruction = 0;
            uint regA = initialRegAVal, regB = 0;
            while (curInstruction < instructions.Count)
            {
                bool jump = false;
                InstructionSet curSet = instructions[curInstruction];
                switch (curSet.Instruction)
                {
                    case Instruction.Half:
                    case Instruction.Triple:
                    case Instruction.Increment:
                        Perform(curSet.Instruction, curSet.Register, curSet.Offset, ref regA, ref regB);
                        ++curInstruction;
                        break;
                    case Instruction.Jump:
                        jump = true;
                        break;
                    case Instruction.JumpIfEven:
                        jump = IsEven(regA, regB, curSet.Register);
                        if (!jump)
                        {
                            ++curInstruction;
                        }
                        break;
                    case Instruction.JumpIfOne:
                        jump = IsOne(regA, regB, curSet.Register);
                        if (!jump)
                        {
                            ++curInstruction;
                        }
                        break;
                }

                if (jump)
                {
                    curInstruction += curSet.Offset;
                }
            }

            if (returnRegister == "a")
            {
                return regA.ToString();
            }
            return regB.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, 0);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, 1);
    }
}