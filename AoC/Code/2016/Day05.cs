using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AoC._2016
{
    class Day05 : Core.Day
    {
        public Day05() { }

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
                    Output = "18f47a30",
                    RawInput =
@"abc"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "05ace8e3",
                    RawInput =
@"abc"
                },
            ];
            return testData;
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool useEmbeddedIndex)
        {
            const int passwordLength = 8;
            string password = string.Empty;
            if (useEmbeddedIndex)
            {
                password = new string('_', passwordLength);
            }
            StringBuilder sb = new();
            string input = inputs.First();
            for (int i = 0, usedIndices = 0; usedIndices < passwordLength; ++i)
            {
                int prevUsed = usedIndices;
                sb = new StringBuilder(input);
                sb.Append(i);
                byte[] inputBytes = Encoding.ASCII.GetBytes(sb.ToString());
                byte[] hashBytes = MD5.HashData(inputBytes);
                string encoded = Convert.ToHexString(hashBytes);
                if (encoded.StartsWith("00000"))
                {
                    // 6th character is the embedded index, 7th character is the character to use
                    if (useEmbeddedIndex)
                    {
                        if (int.TryParse(encoded[5].ToString(), out int idx))
                        {
                            if (idx >= 0 && idx <= 7 && password[idx] == '_')
                            {
                                StringBuilder sbpwd = new(password);
                                sbpwd[idx] = encoded[6];
                                password = sbpwd.ToString();
                                usedIndices = passwordLength - password.Where(c => c == '_').Count();
                            }
                        }

                    }
                    // append 6th character to password
                    else
                    {
                        ++usedIndices;
                        password += encoded[5];
                    }
                }

                // if (prevUsed != usedIndices)
                // {
                //     Core.Log.WriteLine(Core.Log.ELevel.Spam, $"pwd={password}");
                // }
            }
            return password.ToLower();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);


        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);

    }
}