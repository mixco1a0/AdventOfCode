using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC._2021
{
    class Day08 : Core.Day
    {
        public Day08() { }

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
                    Output = "26",
                    RawInput =
@"be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe
edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc
fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg
fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb
aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea
fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb
dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe
bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef
egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb
gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "5353",
                    RawInput =
@"acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "61229",
                    RawInput =
@"be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe
edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc
fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg
fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb
aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea
fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb
dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe
bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef
egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb
gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce"
                },
            ];
            return testData;
        }

        private class Signal
        {
            public List<string> Patterns { get; set; }
            public List<string> Output { get; set; }

            public int Decode()
            {
                string[] translator = new string[10];
                translator[1] = Patterns.Single(p => p.Length == 2);
                translator[4] = Patterns.Single(p => p.Length == 4);
                translator[7] = Patterns.Single(p => p.Length == 3);
                translator[8] = Patterns.Single(p => p.Length == 7);
                translator[9] = Patterns.Single(p => p.Length == 6 && p.Except(translator[7]).Except(translator[4]).Count() == 1);
                translator[0] = Patterns.Single(p => p.Length == 6 && p != translator[9] && p.Except(translator[7]).Count() == 3);
                translator[6] = Patterns.Single(p => p.Length == 6 && p != translator[9] && p.Except(translator[7]).Count() == 4);
                translator[5] = Patterns.Single(p => p.Length == 5 && translator[6].Except(p).Count() == 1);
                translator[3] = Patterns.Single(p => p.Length == 5 && p != translator[5] && translator[9].Except(p).Count() == 1);
                translator[2] = Patterns.Single(p => p.Length == 5 && p != translator[5] && p != translator[3]);
                for (int i = 0; i < translator.Length; ++i)
                {
                    translator[i] = string.Concat(translator[i].OrderBy(c => c));
                }

                StringBuilder code = new();
                foreach (string output in Output)
                {
                    code.Append(translator.Select((translated, idx) => new { translated = translated, idx = idx }).Single(p => p.translated == output).idx);
                }
                return int.Parse(code.ToString());
            }

            public static Signal Parse(string input)
            {
                Signal signal = new();
                string[] split = Util.String.Split(input, '|');
                signal.Patterns = [.. Util.String.Split(split[0], ' ').OrderBy(s => s.Length)];
                signal.Output = [.. Util.String.Split(split[1], ' ').Select(s => string.Concat(s.OrderBy(c => c)))];
                return signal;
            }
        }

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool decode)
        {
            List<Signal> signals = [.. inputs.Select(Signal.Parse)];
            HashSet<int> uniqueValues = [2, 4, 3, 7];
            int sum = 0;
            foreach (Signal signal in signals)
            {
                if (decode)
                {
                    sum += signal.Decode();
                }
                else
                {
                    sum += signal.Output.Where(p => uniqueValues.Contains(p.Length)).Count();
                }
            }
            return sum.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}