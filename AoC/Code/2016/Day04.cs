using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2016
{
    class Day04 : Day
    {
        public Day04() { }
        public override string GetSolutionVersion(Part part)
        {
            switch (part)
            {
                // case Part.One:
                //     return "v1";
                // case Part.Two:
                //     return "v1";
                default:
                    return base.GetSolutionVersion(part);
            }
        }
        protected override List<TestDatum> GetTestData()
        {
            List<TestDatum> testData = new List<TestDatum>();
            testData.Add(new TestDatum
            {
                TestPart = Part.One,
                Output = "1514",
                RawInput =
@"aaaaa-bbb-z-y-x-123[abxyz]
a-b-c-d-e-f-g-h-987[abcde]
not-a-real-room-404[oarel]
totally-real-room-200[decoy]"
            });
            testData.Add(new TestDatum
            {
                TestPart = Part.Two,
                Output = "",
                RawInput =
@""
            });
            return testData;
        }

        private record Room(string Name, long SectorID, string Checksum)
        {
            static public Room Parse(string input)
            {
                string[] split = input.Split("-[],".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                StringBuilder sb = new StringBuilder();
                split.SkipLast(2).ToList().ForEach(s => sb.Append(s));
                return new Room(sb.ToString(), long.Parse(split.TakeLast(2).First()), split.Last());
            }

            private struct SortHelper
            {
                public char c;
                public int count;
            }

            public bool IsValid()
            {
                SortHelper[] sorter = new SortHelper[26];
                foreach(char c in Name)
                {
                    int index = c - 'a';
                    sorter[c - 'a'].c = c;
                    ++sorter[c - 'a'].count;
                }
                var postSort = sorter.OrderBy(s => s.c).OrderByDescending(s => s.count);
                for (int i = 0; i < Checksum.Length; ++i)
                {
                    if (Checksum[i] != postSort.ElementAt(i).c)
                    {
                        return false;
                    }
                }
                return true;
            }
        };

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            List<Room> allRooms = inputs.Select(Room.Parse).ToList();
            long sectorIdSum = 0;
            foreach(Room room in allRooms)
            {
                if (room.IsValid())
                {
                    sectorIdSum += room.SectorID;
                }
            }
            return sectorIdSum.ToString();
        }

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            return "";
        }
    }
}