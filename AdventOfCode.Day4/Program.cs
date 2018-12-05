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
                .Aggregate((best, next) => best.Sum > next.Sum ? best : next)
                .Group;
            var bestMinute = bestGuard
                .SelectMany(s => s.Sleeping.Select((b, i) => new {b, i}))
                .GroupBy(a => a.i)
                .Select(g => new {Group = g, Count = g.Count(a => a.b)})
                .Aggregate((best, next) => best.Count > next.Count ? best : next)
                .Group;

            return bestGuard.Key * bestMinute.Key;
        }

        private static int FrequentSleeping()
        {
            return ParseShifts()
                .GroupBy(s => s.Guard)
                .Select(g => Enumerable.Range(0, 60)
                    .Select(i => new {Minute = i, Count = g.Count(s => s.Sleeping[i])})
                    .Aggregate(new {Guard = g.Key, Minute = 0, Count = 0},
                        (best, next) => best.Count > next.Count ? best : new {best.Guard, next.Minute, next.Count}))
                .Aggregate(new {Guard = 0, Minute = 0, Count = 0},
                    (best, next) => best.Count > next.Count ? best : next, best => best.Guard * best.Minute);
        }

        private static void Main()
        {
            Console.WriteLine(MostSleeping());
            Console.WriteLine(FrequentSleeping());
            Console.ReadKey(true);
        }
    }
}
