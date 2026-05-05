using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2015
{
    class Day13 : Core.Day
    {
        public Day13() { }

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
                    Output = "330",
                    RawInput =
@"Alice would gain 54 happiness units by sitting next to Bob.
Alice would lose 79 happiness units by sitting next to Carol.
Alice would lose 2 happiness units by sitting next to David.
Bob would gain 83 happiness units by sitting next to Alice.
Bob would lose 7 happiness units by sitting next to Carol.
Bob would lose 63 happiness units by sitting next to David.
Carol would lose 62 happiness units by sitting next to Alice.
Carol would gain 60 happiness units by sitting next to Bob.
Carol would gain 55 happiness units by sitting next to David.
David would gain 46 happiness units by sitting next to Alice.
David would lose 7 happiness units by sitting next to Bob.
David would gain 41 happiness units by sitting next to Carol."
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

        private record Units(string Name, int Happiness) { }

        private static int ArrangeSeats(Dictionary<string, List<Units>> people, string person, List<string> peopleSitting, int tab)
        {
            if (!peopleSitting.Contains(person))
            {
                peopleSitting.Add(person);

                List<string> usables = [.. people.Keys.Where(k => !peopleSitting.Contains(k))];
                if (usables.Count == 0)
                {
                    usables.Add(peopleSitting.First());
                }
                int max = int.MinValue;
                foreach (string usable in usables)
                {
                    List<Units> units = people.First(p => p.Key == person).Value;
                    string nextTo = peopleSitting.First();

                    int h1 = 0;
                    if (people.Keys.Count > peopleSitting.Count)
                    {
                        Units usableUnit = units.First(u => u.Name == usable);
                        h1 = usableUnit.Happiness;
                        nextTo = usableUnit.Name;
                    }
                    else
                    {
                        h1 = units.First(u => u.Name == nextTo).Happiness;
                    }
                    int curHappiness = 0;
                    curHappiness += h1;
                    curHappiness += people.First(p => p.Key == nextTo).Value.First(u => u.Name == person).Happiness;
                    // Log(Core.Log.ELevel.Spam, $"{new string('\t', tab)}{person} <={curHappiness}=> {nextTo}");
                    curHappiness += ArrangeSeats(people, nextTo, [.. peopleSitting], tab + 1);

                    max = Math.Max(max, curHappiness);
                }
                return max;
            }
            return 0;
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            Dictionary<string, List<Units>> people = [];
            foreach (string input in inputs)
            {
                string[] split = input.Split(" .".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (!people.TryGetValue(split[0], out List<Units> value))
                {
                    value = [];
                    people[split[0]] = value;
                }

                value.Add(new Units(split.Last(), (split[2] == "gain" ? 1 : -1) * int.Parse(split[3])));
            }
            int max = int.MinValue;
            foreach (var pair in people)
            {
                int h = ArrangeSeats(people, pair.Key, [], 0);
                max = Math.Max(max, h);
            }
            return max.ToString();
        }

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            Dictionary<string, List<Units>> people = [];
            foreach (string input in inputs)
            {
                string[] split = input.Split(" .".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (!people.TryGetValue(split[0], out List<Units> value))
                {
                    value = [];
                    people[split[0]] = value;
                }

                value.Add(new Units(split.Last(), (split[2] == "gain" ? 1 : -1) * int.Parse(split[3])));
            }

            string me = "Me";
            foreach (string person in people.Keys)
            {
                people[person].Add(new Units(me, 0));
            }
            people[me] = [];
            foreach (string person in people.Keys)
            {
                if (person == me)
                {
                    continue;
                }
                people[me].Add(new Units(person, 0));
            }

            int max = int.MinValue;
            foreach (var pair in people)
            {
                int h = ArrangeSeats(people, pair.Key, [], 0);
                max = Math.Max(max, h);
            }
            return max.ToString();
        }
    }
}