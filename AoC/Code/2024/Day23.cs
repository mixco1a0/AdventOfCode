using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2024
{
    class Day23 : Core.Day
    {
        public Day23() { }

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
                    Output = "7",
                    RawInput =
@"kh-tc
qp-kh
de-cg
ka-co
yn-aq
qp-ub
cg-tb
vc-aq
tb-ka
wh-tc
yn-cg
kh-ub
ta-co
de-co
tc-td
tb-wq
wh-td
ta-ka
td-qp
aq-cg
wq-ub
ub-vc
de-ta
wq-aq
wq-vc
wh-yn
ka-de
kh-ta
co-tc
wh-qp
tb-vc
td-yn"
                },
                new Core.TestDatum
                {
                    TestPart = Core.Part.Two,
                    Output = "co,de,ka,ta",
                    RawInput =
@"kh-tc
qp-kh
de-cg
ka-co
yn-aq
qp-ub
cg-tb
vc-aq
tb-ka
wh-tc
yn-cg
kh-ub
ta-co
de-co
tc-td
tb-wq
wh-td
ta-ka
td-qp
aq-cg
wq-ub
ub-vc
de-ta
wq-aq
wq-vc
wh-yn
ka-de
kh-ta
co-tc
wh-qp
tb-vc
td-yn"
                },
            ];
            return testData;
        }

        private record Connection(ushort First, ushort Second)
        {
            public static Connection Parse(string connection)
            {
                string[] split = Util.String.Split(connection, '-');
                ushort first = (ushort)(split[0][0] - 'a');
                first <<= 8;
                first |= (ushort)(split[0][1] - 'a');
                ushort second = (ushort)(split[1][0] - 'a');
                second <<= 8;
                second |= (ushort)(split[1][1] - 'a');
                return new Connection(first, second);
            }

            public override string ToString()
            {
                char f2 = (char)((First & 0xff) + 'a');
                char f1 = (char)((First >> 8) + 'a');
                char s2 = (char)((Second & 0xff) + 'a');
                char s1 = (char)((Second >> 8) + 'a');
                return $"{f1}{f2}-{s1}{s2} => {First:X4}-{Second:X4}";
            }

            public IEnumerable<ushort> All()
            {
                return [First, Second];
            }
        }

        static string FromCompressed(ushort node)
        {
            char f2 = (char)((node & 0xff) + 'a');
            char f1 = (char)((node >> 8) + 'a');
            return $"{f1}{f2}";
        }

        static ulong CompressTriplet(ushort a, ushort b, ushort c)
        {
            List<ushort> ushorts = [a, b, c];
            ushorts.Sort();
            return (ulong)ushorts[0] << 32 | (ulong)ushorts[1] << 16 | (ulong)ushorts[2];
        }

        HashSet<int> Checked { get; set; }

        int GetConnectedComputers(Dictionary<ushort, List<ushort>> allComputers, ushort curComputer, HashSet<ushort> curConnectedComputers, ref HashSet<ushort> maxConnectedComputers)
        {
            HashSet<ushort> curComputers = [.. allComputers[curComputer]];

            bool isConnected = true;
            foreach (ushort computer in curConnectedComputers)
            {
                isConnected &= allComputers[computer].Contains(curComputer);
            }

            if (isConnected)
            {
                curConnectedComputers.Add(curComputer);

                // prevent checking paths that have already been checked before
                int hashCode = 0;
                foreach (ushort u in curConnectedComputers.ToList().Order())
                {
                    hashCode = HashCode.Combine(hashCode, u);
                }
                if (Checked.Contains(hashCode))
                {
                    return 0;
                }
                Checked.Add(hashCode);

                int maxConnected = 0;
                foreach (ushort nextComputer in curComputers)
                {
                    if (curConnectedComputers.Contains(nextComputer))
                    {
                        continue;
                    }
                    int newMax = GetConnectedComputers(allComputers, nextComputer, curConnectedComputers, ref maxConnectedComputers);
                    if (newMax > maxConnectedComputers.Count)
                    {
                        maxConnectedComputers = [.. curConnectedComputers];
                        maxConnected = newMax;
                        //Log($"new max: {string.Join(",", maxConnectedComputers.ToList().Order().Select(c => FromCompressed(c)))}");
                    }
                }
                curConnectedComputers.Remove(curComputer);
                return maxConnected;
            }

            return curConnectedComputers.Count;
        }

        private string SharedSolution(List<string> inputs, Dictionary<string, string> variables, bool findLanParty)
        {
            List<Connection> connections = [.. inputs.Select(Connection.Parse)];
            Dictionary<ushort, List<Connection>> groups = [];
            foreach (Connection c in connections)
            {
                if (!groups.TryGetValue(c.First, out List<Connection> value))
                {
                    value = [];
                    groups[c.First] = value;
                }

                if (!groups.TryGetValue(c.Second, out List<Connection> value2))
                {
                    value2 = [];
                    groups[c.Second] = value2;
                }

                groups[c.First].Add(c);
                groups[c.Second].Add(c);
            }

            Dictionary<ushort, List<ushort>> optimized = groups.ToDictionary(pair => pair.Key, pair => pair.Value.SelectMany(v => v.All()).Distinct().Where(v => v != pair.Key).ToList());

            if (!findLanParty)
            {
                HashSet<ulong> triplets = [];
                foreach (var kvp in optimized)
                {
                    for (int i = 0; i < kvp.Value.Count - 1; ++i)
                    {
                        ushort node1 = kvp.Value[i];
                        for (int j = i + 1; j < kvp.Value.Count; ++j)
                        {
                            ushort node2 = kvp.Value[j];
                            if (optimized[node1].Contains(node2))
                            {
                                ulong temp = CompressTriplet(kvp.Key, node1, node2);
                                if (!triplets.Contains(temp))
                                {
                                    // Log($"Found: {FromCompressed(kvp.Key)} - {FromCompressed(node1)} - {FromCompressed(node2)}");
                                    triplets.Add(temp);
                                }
                            }
                        }
                    }
                }

                const ushort tCheck = ('t' - 'a');
                long tripletCount = 0;
                foreach (ulong triplet in triplets)
                {
                    ushort a = (ushort)((triplet & 0xffff) >> 8);
                    ushort b = (ushort)((triplet >> 16 & 0xffff) >> 8);
                    ushort c = (ushort)((triplet >> 32 & 0xffff) >> 8);
                    if ((a == tCheck) || (b == tCheck) || (c == tCheck))
                    {
                        ++tripletCount;
                    }
                }
                return tripletCount.ToString();
            }

            Checked = [];
            HashSet<ushort> maxConnectedComputers = [];
            foreach (ushort computer in optimized.Keys)
            {
                GetConnectedComputers(optimized, computer, [], ref maxConnectedComputers);
            }
            return string.Join(",", maxConnectedComputers.ToList().Order().Select(c => FromCompressed(c)));
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, false);

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
            => SharedSolution(inputs, variables, true);
    }
}