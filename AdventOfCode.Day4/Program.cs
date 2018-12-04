using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day4
{
    public class Shift
    {
        public Shift(int guard)
        {
            Guard = guard;
            Sleeping = new bool[60];
        }

        public int Guard { get; set; }
        public bool[] Sleeping { get; }
    }

    internal static class Program
    {
        private static IEnumerable<Shift> ParseShifts()
        {
            var regex = new Regex(@"^\[(?<timestamp>\d{4}-\d{2}-\d{2} \d{2}:\d{2})\] (?:Guard #(?<guard>\d+) )?(?<event>begins shift|falls asleep|wakes up)$");
            var lines = File.ReadAllLines("input.txt")
                .OrderBy(l => l)
                .Select(l => regex.Match(l));

            var allShifts = new List<Shift>();
            var lastMinute = 0;

            foreach (var line in lines)
            {
                if (line.Groups["event"].Value == "begins shift")
                {
                    allShifts.Add(new Shift(int.Parse(line.Groups["guard"].Value)));
                }
                else if (line.Groups["event"].Value == "falls asleep")
                {
                    lastMinute = DateTime.Parse(line.Groups["timestamp"].Value).Minute;
                }
                else if (line.Groups["event"].Value == "wakes up")
                {
                    for (var i = lastMinute; i < DateTime.Parse(line.Groups["timestamp"].Value).Minute; i++)
                    {
                        allShifts.Last().Sleeping[i] = true;
                    }
                }
            }

            return allShifts;
        }

        private static int MostSleeping()
        {
            var bestGuard = ParseShifts()
                .GroupBy(s => s.Guard)
                .Select(g => new {Group = g, Sum = g.Sum(s => s.Sleeping.Count(b => b))})
                .Aggregate((best, current) => best.Sum > current.Sum ? best : current)
                .Group;
            var bestMinute = bestGuard
                .SelectMany(s => s.Sleeping.Select((b, i) => new {b, i}))
                .GroupBy(a => a.i)
                .Select(g => new {Group = g, Count = g.Count(a => a.b)})
                .Aggregate((best, current) => best.Count > current.Count ? best : current)
                .Group;

            return bestGuard.Key * bestMinute.Key;
        }

        private static int FrequentSleeping()
        {
            var bestGuard = ParseShifts()
                .GroupBy(s => s.Guard)
                .Select(g => new {Group = g, Max = Enumerable.Range(0, 60).Max(i => g.Count(s => s.Sleeping[i]))})
                .Aggregate((best, current) => best.Max > current.Max ? best : current)
                .Group;
            var bestMinute = Enumerable.Range(0, 60)
                .Select(i => new {Minute = i, Count = bestGuard.Count(g => g.Sleeping[i])})
                .Aggregate((best, current) => best.Count > current.Count ? best : current)
                .Minute;

            return bestGuard.Key * bestMinute;
        }

        private static void Main()
        {
            Console.WriteLine(MostSleeping());
            Console.WriteLine(FrequentSleeping());
            Console.ReadKey(true);
        }
    }
}
