using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.Day1
{
    internal static class Program
    {
        private static int FrequencySum()
        {
            return File.ReadAllLines("input.txt").Sum(int.Parse);
        }

        private static int FrequencyTwice()
        {
            var frequency = 0;
            var frequencies = new HashSet<int> {0};
            var changes = File.ReadAllLines("input.txt")
                .Select(int.Parse)
                .ToList();

            while (true)
            {
                foreach (var change in changes)
                {
                    frequency += change;
                    if (!frequencies.Add(frequency))
                    {
                        return frequency;
                    }
                }
            }
        }

        private static void Main()
        {
            Console.WriteLine(FrequencySum());
            Console.WriteLine(FrequencyTwice());
            Console.ReadKey(true);
        }
    }
}
