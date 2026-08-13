# 🎄 Advent of Code 🌟

My solutions for the annual **[Advent of Code](https://adventofcode.com)** challenges, written in **[C# (.NET 10.0+)](https://microsoft.com)**.

## 📁 Repository Structure

```text
.
├─ AoC/
│  ├─ Code/
│  │  ├─ 20XX/                   # Directory with year specific solutions
│  │  │  └─ DayXX.cs
│  │  ├─ Algorithm/              # Shared algorithms
│  │  ├─ Base/                   # Shared base classes used frequently
│  │  ├─ Core/                   # Core functionality 
│  │  └─ Util/                   # Utility functions used by solutions and system as a whole
│  ├─ Data/                      # (ignored)
│  │  ├─ 20XX/
│  │  │  ├─ In                   # Files containing daily input for puzzles
│  │  │  │  └─ dayXX.txt
│  │  │  └─ Out                  # Files containing expected answers for daily puzzles
│  │  │     └─ dayXX.txt
│  │  ├─ config.json             # Config used in conjunction with run.bat
│  │  ├─ debugger_config.json    # Config used when running from vs code
│  │  ├─ default_config.json     # Generated on initial run to provide template
│  │  └─ perfdata.json           # File containing performance data for all runs
│  ├─ AoC.csproj
│  ├─ EntryPoint.cs              # The entry point to running puzzle solutions
│  ├─ perf_run.bat               # Run default configuration for performance testing on all solutions for current year
│  ├─ perf_show.bat              # Display existing performance metrics for current year using compact output
│  ├─ run.bat                    # Run program using config.json that lives in the Data directory
│  └─ todo.txt                   # Backlog of tasks that need to be addressed
└─ README.md
```