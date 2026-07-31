using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC._2016
{
    class Day18 : Core.Day
    {
        public Day18() { }

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
                    Variables = new Dictionary<string, string>() { { nameof(_RowCount), "3" } },
                    Output = "6",
                    RawInput =
@"..^^."
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Variables = new Dictionary<string, string>() { { nameof(_RowCount), "10" } },
                    Output = "38",
                    RawInput =
@".^^.^.^^^^"
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
        private static string _RowCount { get; }
#pragma warning restore IDE1006 // Naming Styles

        private static char Safe { get { return '.'; } }
        private static char Trap { get { return '^'; } }

        public static char GetTile(string prevRow, int pos)
        {
            char l = Safe, r = Safe;

            // left
            if (pos != 0)
            {
                l = prevRow[pos - 1];
            }

            //right
            if (pos < prevRow.Length - 1)
            {
                r = prevRow[pos + 1];
            }

            if (l != r)
            {
                return Trap;
            }
            return Safe;
        }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, int defaultRowCount)
        {
            GetVariable(nameof(_RowCount), defaultRowCount, variables, out int rowCount);
            string prevRow = inputs.First();
            StringBuilder allTiles = new();
            for (int r = 0; r < rowCount; ++r)
            {
                //DebugWriteLine($"Row {r,2} - {prevRow}");
                allTiles.AppendLine(prevRow);
                StringBuilder sb = new();
                for (int c = 0; c < prevRow.Length; ++c)
                {
                    sb.Append(GetTile(prevRow, c));
                }
                prevRow = sb.ToString();
            }
            return allTiles.ToString().Count(c => c == Safe).ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, 40);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, 400000);
    }
}