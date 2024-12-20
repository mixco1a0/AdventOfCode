using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2015
{
    class Day15 : Core.Day
    {
        public Day15() { }

        public override string GetSolutionVersion(Core.Part part)
        {
            switch (part)
            {
                case Core.Part.One:
                    return "v1";
                case Core.Part.Two:
                    return "v1";
                default:
                    return base.GetSolutionVersion(part);
            }
        }

        protected override List<Core.TestDatum> GetTestData()
        {
            List<Core.TestDatum> testData = new List<Core.TestDatum>();
            testData.Add(new Core.TestDatum
            {
                TestPart = Core.Part.One,
                Output = "62842880",
                RawInput =
@"Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8
Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3"
            });
            testData.Add(new Core.TestDatum
            {
                TestPart = Core.Part.Two,
                Output = "57600000",
                RawInput =
@"Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8
Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3"
            });
            return testData;
        }

        private record Ingredient(string Name, int Capacity, int Durability, int Flavor, int Texture, int Calories)
        {
            static public Ingredient Parse(string input)
            {
                string[] split = input.Split(" :,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                int[] values = split.Where(s => { int v; return int.TryParse(s, out v); }).Select(int.Parse).ToArray();
                return new Ingredient(split[0], values[0], values[1], values[2], values[3], values[4]);
            }
        }

        private bool Increment(ref List<int> allCounts)
        {
            if (allCounts.First() == 100)
            {
                return false;
            }

            --allCounts[allCounts.Count - 1];
            ++allCounts[allCounts.Count - 2];

            if (allCounts.Last() < 0)
            {
                for (int i = allCounts.Count - 3; i >= 0; --i)
                {
                    ++allCounts[i];
                    if (allCounts[i] > 100)
                    {
                        allCounts[i] = 0;
                    }
                    else
                    {
                        break;
                    }
                }

                int max = 0;
                IEnumerable<int> others = allCounts.Take(allCounts.Count - 2);
                foreach (int other in others)
                {
                    max += other;
                }

                allCounts[allCounts.Count - 1] = 100 - max;
                allCounts[allCounts.Count - 2] = 0;
            }

            return true;
        }

        private int Score(List<Ingredient> ingredients, List<int> counts, bool countCalories)
        {
            int capacityScore = 0, durabilityScore = 0, flavorScore = 0, textureScore = 0, calorieScore = 0;
            for (int i = 0; i < ingredients.Count; ++i)
            {
                capacityScore += ingredients[i].Capacity * counts[i];
                durabilityScore += ingredients[i].Durability * counts[i];
                flavorScore += ingredients[i].Flavor * counts[i];
                textureScore += ingredients[i].Texture * counts[i];
                calorieScore += ingredients[i].Calories * counts[i];
            }
            int score =  (capacityScore > 0 ? capacityScore : 0) *
                         (durabilityScore > 0 ? durabilityScore : 0) *
                         (flavorScore > 0 ? flavorScore : 0) *
                         (textureScore > 0 ? textureScore : 0);

            if (countCalories)
            {
                if (calorieScore == 500)
                {
                    return score;
                }
                return 0;
            }
            else
            {
                return score;
            }
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            List<Ingredient> allIngredients = inputs.Select(Ingredient.Parse).ToList();
            List<int> allCounts = new List<int>();
            for (int i = 0; i < allIngredients.Count; ++i)
            {
                allCounts.Add(i == allIngredients.Count - 1 ? 100 : 0);
            }

            long score = int.MinValue;
            while (true)
            {
                if (!Increment(ref allCounts))
                {
                    break;
                }
                score = Math.Max(score, Score(allIngredients, allCounts, false));
            }

            return score.ToString();
        }

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            List<Ingredient> allIngredients = inputs.Select(Ingredient.Parse).ToList();
            List<int> allCounts = new List<int>();
            for (int i = 0; i < allIngredients.Count; ++i)
            {
                allCounts.Add(i == allIngredients.Count - 1 ? 100 : 0);
            }

            long score = int.MinValue;
            while (true)
            {
                if (!Increment(ref allCounts))
                {
                    break;
                }
                score = Math.Max(score, Score(allIngredients, allCounts, true));
            }

            return score.ToString();
        }
    }
}