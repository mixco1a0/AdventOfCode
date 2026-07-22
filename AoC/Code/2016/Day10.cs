using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2016
{
    class Day10 : Core.Day
    {
        public Day10() { }

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
                    Variables = new Dictionary<string, string> { { nameof(_ChipOne), "2" }, { nameof(_ChipTwo), "5" } },
                    Output = "2",
                    RawInput =
@"value 5 goes to bot 2
bot 2 gives low to bot 1 and high to bot 0
value 3 goes to bot 1
bot 1 gives low to output 1 and high to bot 0
bot 0 gives low to output 2 and high to output 0
value 2 goes to bot 2"
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
        private static string _ChipOne { get; }
        private static string _ChipTwo { get; }
#pragma warning restore IDE1006 // Naming Styles

        public class Bot
        {
            public Bot()
            {
                Chips = [int.MaxValue, int.MaxValue];
            }

            public void Assign(int number)
            {
                for (int i = 0; i < Chips.Count; ++i)
                {
                    if (Chips[i] == int.MaxValue)
                    {
                        Chips[i] = number;
                        break;
                    }
                }
                Chips.Sort();
            }

            public bool CanGive()
            {
                return Chips[0] != int.MaxValue && Chips[1] != int.MaxValue;
            }

            public void Give(ref Bot low, ref Bot high)
            {
                low.Assign(Chips.First());
                high.Assign(Chips.Last());

                Chips.Clear();
                Chips.Add(int.MaxValue);
                Chips.Add(int.MaxValue);
            }

            public bool IsComparing(int chipOne, int chipTwo)
            {
                return Chips[0] == chipOne && Chips[1] == chipTwo;
            }

            public List<int> Chips { get; set; }
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool useOutputs)
        {
            GetVariable(nameof(_ChipOne), 17, variables, out int chipOne);
            GetVariable(nameof(_ChipTwo), 61, variables, out int chipTwo);

            Dictionary<int, Bot> bots = [];
            foreach (string i in inputs.Where(i => i.StartsWith("value")).ToList())
            {
                List<int> values = [.. i.Split(' ').Where(s => { return int.TryParse(s, out int tryParse); }).Select(s => int.Parse(s))];
                if (!bots.TryGetValue(values[1], out Bot value))
                {
                    value = new Bot();
                    bots[values[1]] = value;
                }

                value.Assign(values[0]);

                if (value.IsComparing(chipOne, chipTwo) && !useOutputs)
                {
                    return values[1].ToString();
                }
            }

            Dictionary<int, Bot> outputs = [];
            List<string> instructions = [.. inputs.Where(i => !i.StartsWith("value"))];
            while (instructions.Count > 0)
            {
                string instruction = instructions.First();
                string[] split = Util.String.Split(instruction, ' ');
                int giverId = int.Parse(split[1]);
                if (!bots.ContainsKey(giverId))
                {
                    instructions.RemoveAt(0);
                    instructions.Add(instruction);
                    continue;
                }

                Bot giver = bots[giverId];
                if (!giver.CanGive())
                {
                    instructions.RemoveAt(0);
                    instructions.Add(instruction);
                    continue;
                }

                Bot low;
                int lowId = int.Parse(split[6]);
                bool checkLow = false;
                if (split[5] == "output")
                {
                    if (!outputs.TryGetValue(lowId, out Bot value))
                    {
                        value = new Bot();
                        outputs[lowId] = value;
                    }
                    low = value;
                }
                else
                {
                    checkLow = true;
                    if (!bots.TryGetValue(lowId, out Bot value))
                    {
                        value = new Bot();
                        bots[lowId] = value;
                    }
                    low = value;
                }

                Bot high;
                int highId = int.Parse(split[11]);
                bool checkHigh = false;
                if (split[10] == "output")
                {
                    if (!outputs.TryGetValue(highId, out Bot value))
                    {
                        value = new Bot();
                        outputs[highId] = value;
                    }
                    high = value;
                }
                else
                {
                    checkHigh = true;
                    if (!bots.TryGetValue(highId, out Bot value))
                    {
                        value = new Bot();
                        bots[highId] = value;
                    }
                    high = value;
                }

                giver.Give(ref low, ref high);
                if (checkLow && !useOutputs)
                {
                    if (low.IsComparing(chipOne, chipTwo))
                    {
                        return lowId.ToString();
                    }
                }
                if (checkHigh && !useOutputs)
                {
                    if (high.IsComparing(chipOne, chipTwo))
                    {
                        return highId.ToString();
                    }
                }

                instructions.RemoveAt(0);
            }

            return (outputs[0].Chips.First() * outputs[1].Chips.First() * outputs[2].Chips.First()).ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}