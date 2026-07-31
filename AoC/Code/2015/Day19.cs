using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2015
{
    class Day19 : Core.Day
    {
        public Day19() { }

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
                    Output = "4",
                    RawInput =
@"H => HO
H => OH
O => HH

HOH"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Output = "7",
                    RawInput =
@"H => HO
H => OH
O => HH

HOHOHO"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "3",
                    RawInput =
@"e => H
e => O
H => HO
H => OH
O => HH

HOH"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "6",
                    RawInput =
@"e => H
e => O
H => HO
H => OH
O => HH

HOHOHO"
                },
            ];
            return testData;
        }

        public record Replacement(string Pre, string Post) { }

        private static int Fabricate(string molecule, List<Replacement> replacements, string target)
        {
            int minSteps = int.MaxValue;
            HashSet<string> unique = [];
            Fabricate(molecule, 0, replacements, target, true, ref minSteps, ref unique);
            Fabricate(molecule, 0, replacements, target, false, ref minSteps, ref unique);
            return minSteps;
        }

        private static void Fabricate(string molecule, int steps, List<Replacement> replacements, string target, bool greedy, ref int minSteps, ref HashSet<string> unique)
        {
            if (minSteps < int.MaxValue)
            {
                return;
            }

            if (molecule == target)
            {
                if (steps != minSteps && steps < minSteps)
                {
                    // Core.Log.WriteLine(Core.Log.ELevel.Spam, $"New min found {steps}");
                }
                minSteps = Math.Min(minSteps, steps);
                return;
            }

            if (steps >= minSteps)
            {
                return;
            }

            if (!unique.Add(molecule))
            {
                return;
            }

            List<Replacement> curUsable = [.. replacements.Where(r => molecule.Contains(r.Post, StringComparison.CurrentCulture))];
            if (greedy)
            {
                foreach (Replacement replacement in curUsable)
                {
                    string cur = molecule.Replace(replacement.Post, replacement.Pre);
                    if (cur != molecule)
                    {
                        Fabricate(cur, steps + molecule.Split(replacement.Post).Length - 1, replacements, target, greedy, ref minSteps, ref unique);
                    }
                }
            }
            else
            {
                foreach (Replacement replacement in curUsable)
                {
                    for (int i = molecule.IndexOf(replacement.Post); i >= 0 && i < molecule.Length; i = molecule.IndexOf(replacement.Post, i + 1))
                    {
                        string cur = molecule.Remove(i, replacement.Post.Length).Insert(i, replacement.Pre);
                        if (cur.Length <= molecule.Length)
                        {
                            Fabricate(cur, steps + 1, replacements, target, greedy, ref minSteps, ref unique);
                        }
                    }
                }
            }
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool runFabrication)
        {
            List<Replacement> replacements = [];
            string molecule = "";
            foreach (string input in inputs)
            {
                if (input.Contains("=>"))
                {
                    string[] split = [.. Util.String.Split(input, "=>").Select(_ => _.Trim())];
                    replacements.Add(new Replacement(split[0], split[1]));
                }
                else if (input.Trim().Length > 0)
                {
                    molecule = input;
                }
            }

            if (runFabrication)
            {
                return Fabricate(molecule, replacements, "e").ToString();
            }
            
            HashSet<string> unique = [];
            foreach (Replacement replacement in replacements)
            {
                for (int i = molecule.IndexOf(replacement.Pre); i >= 0 && i < molecule.Length; i = molecule.IndexOf(replacement.Pre, i + 1))
                {
                    string cur = molecule.Remove(i, replacement.Pre.Length).Insert(i, replacement.Post);
                    unique.Add(cur);
                }
            }
            return unique.Count.ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}