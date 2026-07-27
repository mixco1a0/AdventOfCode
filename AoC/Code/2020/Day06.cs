using System.Collections.Generic;
using System.Linq;

namespace AoC._2020
{
    class Day06 : Core.Day
    {
        public Day06() { }

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
                    Output = "11",
                    RawInput =
@"abc

a
b
c

ab
ac

a
a
a
a

b"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "6",
                    RawInput =
@"abc

a
b
c

ab
ac

a
a
a
a

b"
                },
            ];
            return testData;
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            int count = 0;
            string groupInput = "";
            foreach (string input in inputs)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    // group end
                    count += groupInput.ToCharArray().Select(c => c.ToString()).Distinct().Count();
                    groupInput = "";
                    continue;
                }

                groupInput += input;
            }
            count += groupInput.ToCharArray().Select(c => c.ToString()).Distinct().Count();
            return count.ToString();
        }

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            int count = 0;
            List<string> sharedInput = [];
            bool newGroup = true;
            foreach (string input in inputs)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    // group end
                    count += sharedInput.Count();
                    sharedInput.Clear();
                    newGroup = true;
                    continue;
                }

                var cur = input.ToCharArray().Select(c => c.ToString());
                if (newGroup)
                {
                    sharedInput = cur.ToList();
                    newGroup = false;
                }
                else
                {
                    sharedInput = sharedInput.Intersect(cur).ToList();
                }

            }
            count += sharedInput.Count();
            return count.ToString();
        }
    }
}