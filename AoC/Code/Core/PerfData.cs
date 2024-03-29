using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Core
{
    public class PerfStat
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public double Avg { get; set; }
        public long Count { get; set; }

        public PerfStat()
        {
            Min = double.MaxValue;
            Max = double.MinValue;
            Avg = 0.0;
            Count = 0;
        }

        public void AddData(double elapsedMs)
        {
            Min = Math.Min(Min, elapsedMs);
            Max = Math.Max(Max, elapsedMs);

            double avg = Avg * (double)Count;
            avg += elapsedMs;
            ++Count;
            Avg = avg / (double)Count;
        }
    }

    public class PerfVersion
    {
        public Dictionary<string, PerfStat> VersionData { get; set; }

        public PerfVersion()
        {
            VersionData = new Dictionary<string, PerfStat>();
        }

        public void AddData(string version, double elapsedMs)
        {
            if (version != "v0")
            {
                if (!VersionData.ContainsKey(version))
                {
                    VersionData[version] = new PerfStat();
                }
                VersionData[version].AddData(elapsedMs);
            }
            VersionData = VersionData.OrderByDescending(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public PerfStat GetData(string version)
        {
            if (VersionData.ContainsKey(version))
            {
                return VersionData[version];
            }
            return null;
        }
    }

    public class PerfPart
    {
        public Dictionary<Part, PerfVersion> PartData { get; set; }

        public PerfPart()
        {
            PartData = new Dictionary<Part, PerfVersion>();
        }

        public void AddData(Day day)
        {
            Dictionary<Part, double> results = day.TimeResults;
            foreach (var pair in results)
            {
                if (!PartData.ContainsKey(pair.Key))
                {
                    PartData[pair.Key] = new PerfVersion();
                }
                PartData[pair.Key].AddData(day.GetSolutionVersion(pair.Key), pair.Value);
            }
            PartData = PartData.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public PerfStat Get(Part part, string version)
        {
            if (PartData.ContainsKey(part))
            {
                return PartData[part].GetData(version);
            }
            return null;
        }
    }

    public class PerfDay
    {
        public Dictionary<string, PerfPart> DayData { get; set; }

        public PerfDay()
        {
            DayData = new Dictionary<string, PerfPart>();
        }

        public void AddData(Day day)
        {
            if (!DayData.ContainsKey(day.DayName))
            {
                DayData[day.DayName] = new PerfPart();
            }
            DayData[day.DayName].AddData(day);
            DayData = DayData.OrderByDescending(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public PerfStat Get(string day, Part part, string version)
        {
            if (DayData.ContainsKey(day))
            {
                return DayData[day].Get(part, version);
            }
            return null;
        }
    }

    public class PerfData
    {
        public Dictionary<string, PerfDay> YearData { get; set; }

        public PerfData()
        {
            YearData = new Dictionary<string, PerfDay>();
        }

        public void AddData(Day day)
        {
            if (!YearData.ContainsKey(day.Year))
            {
                YearData[day.Year] = new PerfDay();
            }
            YearData[day.Year].AddData(day);
            YearData = YearData.OrderByDescending(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public PerfStat Get(string year, string day, Part part, string version)
        {
            if (YearData.ContainsKey(year))
            {
                return YearData[year].Get(day, part, version);
            }
            return null;
        }
    }
}