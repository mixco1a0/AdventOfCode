using System;
using System.Collections.Generic;

namespace AoC._2020
{
    class Day05 : Day
    {
        public Day05() { }
        public override string GetSolutionVersion(Part part)
        {
            switch (part)
            {
                case Part.One:
                    return "v1";
                case Part.Two:
                    return "v1";
                default:
                    return base.GetSolutionVersion(part);
            }
        }
        protected override List<TestDatum> GetTestData()
        {
            List<TestDatum> testData = new List<TestDatum>();
            testData.Add(new TestDatum { TestPart = Part.One, Output = "357", RawInput = "FBFBBFFRLR" });
            return testData;
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            int highestId = 0;
            foreach (string input in inputs)
            {
                string binary = input.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1');
                highestId = Math.Max(highestId, Convert.ToInt32(binary, 2));
            }
            return highestId.ToString();
        }

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            HashSet<int> ids = new HashSet<int>();
            foreach (string input in inputs)
            {
                string binary = input.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1');
                ids.Add(Convert.ToInt32(binary, 2));
            }
            foreach (int id in ids)
            {
                if (!ids.Contains(id + 1) && ids.Contains(id + 2))
                {
                    return $"{id + 1}";
                }
            }

            return "";
        }
    }
}