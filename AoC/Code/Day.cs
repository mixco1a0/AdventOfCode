using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC
{
    public enum Part
    {
        One = 1,
        Two = 2
    }

    abstract public class Day
    {
        public static bool UseLogs { get; set; }

        private enum RunType
        {
            Testing,
            Problem
        }

        #region Required Overrides
        protected abstract List<TestDatum> GetTestData();
        protected abstract string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables);
        protected abstract string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables);
        public virtual string GetSolutionVersion(Part part)
        {
            return "v0";
        }
        #endregion

        private string DefaultLogID { get { return new string('.', 27); } }

        public string Year { get; private set; }
        public string DayName { get; private set; }
        public Dictionary<Part, double> TimeResults { get; private set; }

        private string m_logID;
        private Dictionary<string, List<TestDatum>> m_testData;
        private Dictionary<Part, Func<List<string>, Dictionary<string, string>, string>> m_partSpecificFunctions;

        protected Day()
        {
            try
            {
                TimeResults = new Dictionary<Part, double>();

                Year = this.GetType().Namespace.ToString()[^4..];
                DayName = this.GetType().ToString()[^5..].ToLower();
                m_logID = DefaultLogID;
                m_testData = new Dictionary<string, List<TestDatum>>();
                m_testData[DayName] = GetTestData();
                m_partSpecificFunctions = new Dictionary<Part, Func<List<string>, Dictionary<string, string>, string>>
                {
                    {Part.One, RunPart1Solution},
                    {Part.Two, RunPart2Solution}
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        public void Run()
        {
            // file input
            IEnumerable<string> rawFileRead = GetInputFile();

            // run part 1
            RunAll(Part.One, rawFileRead);

            // run part 2
            RunAll(Part.Two, rawFileRead);

            // reset logging
            m_logID = DefaultLogID;
        }

        public void RunProblem(Part part)
        {
            // run part
            RunAll(part, GetInputFile());

            // reset logging
            m_logID = DefaultLogID;
        }

        private IEnumerable<string> GetInputFile()
        {
            // file input
            string fileName = string.Format("{0}.txt", DayName);
            string inputFile = Path.Combine(Util.WorkingDirectory, "Data", Year, fileName);
            // TODO: if the file doesn't exist, download it
            return Util.ConvertInputToList(File.ReadAllText(inputFile));
        }

        private void RunAll(Part part, IEnumerable<string> problemInput)
        {
            // get test data if there is any
            IEnumerable<TestDatum> partSpecificTestData = m_testData[DayName].Where(datum => datum.TestPart == part);

            // run tests if there any
            foreach (TestDatum datum in partSpecificTestData)
            {
                TimedRun(RunType.Testing, part, datum.Input.ToList(), datum.Output, datum.Variables);
            }

            TimeResults[part] = TimedRun(RunType.Problem, part, problemInput.ToList(), "", null);
        }

        private double TimedRun(RunType runType, Part part, List<string> inputs, string expectedOuput, Dictionary<string, string> variables)
        {
            LogFiller();

            Timer timer = new Timer();
            timer.Start();
            RunPart(runType, part, inputs, expectedOuput, variables);
            timer.Stop();

            Log($"{timer.Print()}");
            return timer.GetElapsedMs();
        }

        private void RunPart(RunType runType, Part part, List<string> inputs, string expectedOuput, Dictionary<string, string> variables)
        {
            m_logID = string.Format("{0}|{1}|{2}|part{3}|{4}", Year, DayName, runType == RunType.Problem ? "problem" : "testing", part == Part.One ? "1" : "2", GetSolutionVersion(part));
            try
            {
                string actualOutput = m_partSpecificFunctions[part](inputs, variables);
                if (runType == RunType.Testing && actualOutput != expectedOuput)
                {
                    LogAnswer($"[ERROR] Expected: {expectedOuput} - Actual: {actualOutput} [ERROR]", '!');
                }
                else
                {
                    LogAnswer(actualOutput, runType == RunType.Problem ? '#' : '?');
                }
            }
            catch (Exception e)
            {
                Log($"{e.Message}");
                Log($"{e.StackTrace.Split('\r').FirstOrDefault()}");
                Log($"{e.StackTrace}");
            }
        }

        private void Log(string log)
        {
            if (UseLogs)
            {
                Console.WriteLine($"{Util.GetLogTimeStamp()} [{m_logID}] {log}");
            }
        }

        private void LogFiller()
        {
            if (UseLogs)
            {
                Console.WriteLine($"{Util.GetLogTimeStamp()} [{DefaultLogID}]");
            }
        }

        private void LogAnswer(string answer, char filler)
        {
            if (UseLogs && !string.IsNullOrWhiteSpace(answer))
            {
                string buffer = new string(filler, answer.Length);
                string empty = new string(' ', answer.Length);
                Console.WriteLine($"{Util.GetLogTimeStamp()} [{m_logID}] {new string(filler, 5)}{buffer}{new string(filler, 5)}");
                Console.WriteLine($"{Util.GetLogTimeStamp()} [{m_logID}] {new string(filler, 3)}  {empty }  {new string(filler, 3)}");
                Console.WriteLine($"{Util.GetLogTimeStamp()} [{m_logID}] {new string(filler, 3)}  {answer}  {new string(filler, 3)}");
                Console.WriteLine($"{Util.GetLogTimeStamp()} [{m_logID}] {new string(filler, 3)}  {empty }  {new string(filler, 3)}");
                Console.WriteLine($"{Util.GetLogTimeStamp()} [{m_logID}] {new string(filler, 5)}{buffer}{new string(filler, 5)}");
            }
        }

        protected void DebugWrite(string log)
        {
            if (UseLogs)
            {
                Console.Write($"{Util.GetLogTimeStamp()} [{m_logID}] \t{log}");
            }
        }

        protected void DebugWriteLine(string log)
        {
            if (UseLogs)
            {
                Console.WriteLine($"{Util.GetLogTimeStamp()} [{m_logID}] \t{log}");
            }
        }
    }
}