using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AoC._2016
{
    class Day17 : Core.Day
    {
        public Day17() { }

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
                    Output = "DDRRRD",
                    RawInput =
@"ihgpwlah"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Output = "DDUDRLRRUDRD",
                    RawInput =
@"kglvqrro"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.One,
                    Output = "DRURDRUDDLLDLUURRDULRLDUUDDDRR",
                    RawInput =
@"ulqzkmiv"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "370",
                    RawInput =
@"ihgpwlah"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "492",
                    RawInput =
@"kglvqrro"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "830",
                    RawInput =
@"ulqzkmiv"
                },
            ];
            return testData;
        }

        private record DoorStatus(bool[] Status)
        {
            static public DoorStatus Parse(string input)
            {
                // get md5
                string hash = string.Empty;
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = MD5.HashData(inputBytes);
                hash = Convert.ToHexStringLower(hashBytes);

                // set door status
                bool[] open = new bool[4];
                for (int i = 0; i < 4; ++i)
                {
                    open[i] = !(hash[i] == 'a' || char.IsDigit(hash[i]));
                }
                return new DoorStatus(open);
            }

            static public Base.Vec2[] Directions =
            [
                Util.Grid2.Map.Neighbor[Util.Grid2.Dir.North],
                Util.Grid2.Map.Neighbor[Util.Grid2.Dir.South],
                Util.Grid2.Map.Neighbor[Util.Grid2.Dir.West],
                Util.Grid2.Map.Neighbor[Util.Grid2.Dir.East]
            ];

            static public char[] Letters = ['U', 'D', 'L', 'R'];
        }

        private record WalkStatus(string Path, Base.Vec2 Coords) { }

        private static string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool findLongestPath)
        {
            Queue<WalkStatus> pendingWalks = new();
            pendingWalks.Enqueue(new WalkStatus(inputs.First(), new()));
            int longestPath = 0;
            while (pendingWalks.Count > 0)
            {
                WalkStatus ws = pendingWalks.Dequeue();
                if (ws.Coords.X == 3 && ws.Coords.Y == 3)
                {
                    if (findLongestPath)
                    {
                        longestPath = Math.Max(longestPath, ws.Path.Length);
                        continue;
                    }
                    else
                    {
                        return ws.Path[inputs.First().Length..];
                    }
                }

                DoorStatus ds = DoorStatus.Parse(ws.Path);
                for (int i = 0; i < ds.Status.Length; ++i)
                {
                    if (ds.Status[i])
                    {
                        Base.Vec2 newCoords = ws.Coords + DoorStatus.Directions[i];
                        if (newCoords.X >= 0 && newCoords.X <= 3 && newCoords.Y >= 0 && newCoords.Y <= 3)
                        {
                            pendingWalks.Enqueue(new WalkStatus($"{ws.Path}{DoorStatus.Letters[i]}", newCoords));
                        }
                    }
                }
            }

            return (longestPath - inputs.First().Length).ToString();
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}