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

## 📊 Overall Progress

| Year | Stars | Link to Solutions |
| :---: | :---: | :--- |
| **2025** | ⭐ `0 / 24` | Not Started |
| **2024** | ⭐ `50 / 50` | [2024 Solutions](./AoC/Code/2024/) |
| **2023** | ⭐ `50 / 50` | [2023 Solutions](./AoC/Code/2023/) |
| **2022** | ⭐ `50 / 50` | [2022 Solutions](./AoC/Code/2022/) |
| **2021** | ⭐ `50 / 50` | [2021 Solutions](./AoC/Code/2021/) |
| **2020** | ⭐ `50 / 50` | [2020 Solutions](./AoC/Code/2020/) |
| **2019** | ⭐ `0 / 50` | Not Started |
| **2018** | ⭐ `0 / 50` | Not Started |
| **2017** | ⭐ `0 / 50` | Not Started |
| **2016** | ⭐ `50 / 50` | [2016 Solutions](./AoC/Code/2016/) |
| **2015** | ⭐ `50 / 50` | [2015 Solutions](./AoC/Code/2015/) |

## 🛠️ Getting Started

### Prerequisites
* Install the [.NET 10.0+ SDK](https://microsoft.comdownload)

### Installation & Setup
1. Clone the repository
2. Build the code base
   ```bash
   dotnet build -c Release
   ```
3. Use help command to get details on usage
   ```bash
   bin\Release\net10.0\AoC.exe -help
   ```

## 🚀 Running the Code

The program is expected to be run from within the `./AoC/` directory. 

Run a specific solution using the following command.

```bash
bin\Release\net10.0\AoC.exe -d <day> -n <year>
```

Run using the a config file. By default, a new file will be generated here: `./Data/default_config.json`.

```bash
bin\Release\net10.0\AoC.exe -cf "%cd%\Data\config.json"
```

Run performance testing. The file `./Data/perfdata.json` will be generated with the performance metrics.
See also [perf_run.bat](./AoC/perf_run.bat). 

```bash
bin\Release\net10.0\AoC.exe -skiplatest -runperf -compactperf -perfrecordcount <number_of_runs> -perftimeout <timeout_in_ms> -ignoreconfigfile
```

Show performance testing results. Requires the `./Data/perfdata.json` file. Metrics can be printed in either compact or extended format.
See also [perf_show.bat](./AoC/perf_show.bat).

```bash
bin\Release\net10.0\AoC.exe -skiplatest -showperf -compactperf -ignoreconfigfile
bin\Release\net10.0\AoC.exe -skiplatest -showperf -ignoreconfigfile
```

### Input File Note
Per the Advent of Code [official authorization rules](https://adventofcode.comabout#faq_copying), do not publicly commit your personalized puzzle input files (`input.txt`) to GitHub. Make sure your `.gitignore` excludes input data or local caches. The entire `./AoC/Data/` directory would need to be filled in with personal input files.

## 📜 License
This project is licensed under the MIT License - see the [LICENSE](LICENSE.md) file for details.