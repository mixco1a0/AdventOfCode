using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2020
{
    class Day12 : Day
    {
        public Day12() { }
        protected override List<TestDatum> GetTestData()
        {
            List<TestDatum> testData = new List<TestDatum>();
            testData.Add(new TestDatum
            {
                TestPart = TestPart.One,
                Output = "25",
                RawInput =
@"F10
N3
F7
R90
F11"
            });
            testData.Add(new TestDatum
            {
                TestPart = TestPart.Two,
                Output = "286",
                RawInput =
@"F10
N3
F7
R90
F11"
            });
            return testData;
        }

        protected override string RunPart1Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            char curDir = 'E';
            int x = 0;
            int y = 0;
            foreach (string input in inputs)
            {
                char instruction = input[0..1].First();
                int value = int.Parse(input[1..]);
                switch (instruction)
                {
                    case 'N':
                        y += value;
                        break;
                    case 'S':
                        y -= value;
                        break;
                    case 'E':
                        x += value;
                        break;
                    case 'W':
                        x -= value;
                        break;
                    case 'L':
                        while (value > 0)
                        {
                            switch (curDir)
                            {
                                case 'N':
                                    curDir = 'W';
                                    break;
                                case 'S':
                                    curDir = 'E';
                                    break;
                                case 'E':
                                    curDir = 'N';
                                    break;
                                case 'W':
                                    curDir = 'S';
                                    break;
                            }
                            value -= 90;
                        }
                        break;
                    case 'R':
                        while (value > 0)
                        {
                            switch (curDir)
                            {
                                case 'N':
                                    curDir = 'E';
                                    break;
                                case 'S':
                                    curDir = 'W';
                                    break;
                                case 'E':
                                    curDir = 'S';
                                    break;
                                case 'W':
                                    curDir = 'N';
                                    break;
                            }
                            value -= 90;
                        }
                        break;
                    case 'F':
                        switch (curDir)
                        {
                            case 'N':
                                y += value;
                                break;
                            case 'S':
                                y -= value;
                                break;
                            case 'E':
                                x += value;
                                break;
                            case 'W':
                                x -= value;
                                break;
                        }
                        break;
                }
            }
            return (Math.Abs(x) + Math.Abs(y)).ToString();
        }

        protected override string RunPart2Solution(List<string> inputs, Dictionary<string, string> variables)
        {
            int x = 0;
            int y = 0;
            int waypointX = 10;
            int waypointY = 1;
            foreach (string input in inputs)
            {
                char instruction = input[0..1].First();
                int value = int.Parse(input[1..]);
                switch (instruction)
                {
                    case 'N':
                        waypointY += value;
                        break;
                    case 'S':
                        waypointY -= value;
                        break;
                    case 'E':
                        waypointX += value;
                        break;
                    case 'W':
                        waypointX -= value;
                        break;
                    case 'L':
                        while (value > 0)
                        {
                            int tempX = waypointX;
                            int tempY = waypointY;
                            waypointY = tempX;
                            waypointX = tempY;
                            if (tempX >= 0 && tempY >= 0)
                                waypointX *= -1;
                            if (tempX <= 0 && tempY >= 0)
                                waypointX *= -1;
                            if (tempX <= 0 && tempY <= 0)
                                waypointX *= -1;
                            if (tempX >= 0 && tempY <= 0)
                            {
                                waypointX *= -1;
                            }
                            value -= 90;
                        }
                        break;
                    case 'R':
                        while (value > 0)
                        {
                            int tempX = waypointX;
                            int tempY = waypointY;
                            waypointY = tempX;
                            waypointX = tempY;
                            if (tempX >= 0 && tempY >= 0)
                                waypointY *= -1;
                            if (tempX >= 0 && tempY <= 0)
                            {
                                waypointY *= -1;
                            }
                            if (tempX <= 0 && tempY <= 0)
                                waypointY *= -1;
                            if (tempX <= 0 && tempY >= 0)
                                waypointY *= -1;
                            value -= 90;
                        }
                        break;
                    case 'F':
                        x += waypointX * value;
                        y += waypointY * value;
                        break;
                }
            }
            return (Math.Abs(x) + Math.Abs(y)).ToString();
        }
    }
}