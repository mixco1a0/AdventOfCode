using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AoC._2015
{
    class Day12 : Core.Day
    {
        public Day12() { }

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
                    Output = "6",
                    RawInput =
@"[1,2,3]"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Output = "3",
                    RawInput =
@"[[[3]]]"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Output = "0",
                    RawInput =
@"{}"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "4",
                    RawInput =
"[1,{\"c\":\"red\",\"b\":2},3]"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "0",
                    RawInput =
"{\"d\":\"red\",\"e\":[1,2,3,4],\"f\":5}"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "6",
                    RawInput =
@"[1,2,3]"
                },
            ];
            return testData;
        }

        private static bool HasInvalid(JContainer container, string invalidToken)
        {
            for (JToken token = container.First; token != null; token = token.Next)
            {
                if (token.HasValues && token.Type == JTokenType.Property)
                {
                    for (JToken value = token.First; value != null; value = value.Next)
                    {
                        if (value.Type == JTokenType.String && invalidToken == value.Value<string>())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static int Count(JContainer container, string invalidToken)
        {
            int count = 0;
            if (container != null && (string.IsNullOrWhiteSpace(invalidToken) || !HasInvalid(container, invalidToken)))
            {
                for (JToken token = container.First; token != null; token = token.Next)
                {
                    if (token.HasValues)
                    {
                        count += Count(token as JContainer, invalidToken);
                    }
                    else if (token.Type == JTokenType.Integer)
                    {
                        count += token.Value<int>();
                    }
                }
            }
            return count;
        }

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, string ignore)
        {
            dynamic json = JsonConvert.DeserializeObject(inputs.First());
            return Count(json, ignore).ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, string.Empty);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, "red");
    }
}