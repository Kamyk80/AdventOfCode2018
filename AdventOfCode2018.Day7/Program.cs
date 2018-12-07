using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Day7
{
    internal static class Program
    {
        public class Step
        {
            public bool Started { get; set; }
            public bool Finished { get; set; }
        }
        public class Worker
        {
            public string Task { get; set; }
            public int Time { get; set; }
        }

        private static string StepsOrder()
        {
            var regex = new Regex(@"^Step (?<before>\w) must be finished before step (?<after>\w) can begin.$");
            var rules = File.ReadAllLines("input.txt")
                .Select(l => regex.Match(l))
                .Select(m => new
                {
                    Before = m.Groups["before"].Value,
                    After = m.Groups["after"].Value
                })
                .ToList();
            var steps = rules
                .Select(r => r.Before)
                .Concat(rules
                    .Select(r => r.After))
                .Distinct()
                .ToDictionary(s => s, s => false);
            var order = new StringBuilder();

            while (steps.Any(s => !s.Value))
            {
                var ready = steps
                    .Where(s => !s.Value)
                    .Where(s => rules
                        .Where(r => r.After == s.Key)
                        .All(r => steps[r.Before]))
                    .Select(s => s.Key)
                    .OrderBy(s => s)
                    .First();

                order.Append(ready);
                steps[ready] = true;
            }

            return order.ToString();
        }

        private static int StepsTime()
        {
            var regex = new Regex(@"^Step (?<before>\w) must be finished before step (?<after>\w) can begin.$");
            var rules = File.ReadAllLines("input.txt")
                .Select(l => regex.Match(l))
                .Select(m => new
                {
                    Before = m.Groups["before"].Value,
                    After = m.Groups["after"].Value
                })
                .ToList();
            var steps = rules
                .Select(r => r.Before)
                .Concat(rules
                    .Select(r => r.After))
                .Distinct()
                .ToDictionary(s => s, s => new Step
                {
                    Started = false,
                    Finished = false
                });
            var workers = Enumerable.Range(0, 5)
                .Select(i => new Worker())
                .ToList();
            var time = -1;

            while (steps.Any(s => !s.Value.Finished))
            {
                foreach (var worker in workers)
                {
                    if (worker.Time == 0 && worker.Task != null)
                    {
                        steps[worker.Task].Finished = true;
                        worker.Task = null;
                    }
                }

                foreach (var worker in workers)
                {
                    if (worker.Time == 0)
                    {
                        var ready = steps
                            .Where(s => !s.Value.Started)
                            .Where(s => rules
                                .Where(r => r.After == s.Key)
                                .All(r => steps[r.Before].Finished))
                            .Select(s => s.Key)
                            .OrderBy(s => s)
                            .FirstOrDefault();

                        if (ready != null)
                        {
                            worker.Task = ready;
                            worker.Time = ready[0] - 4;
                            steps[worker.Task].Started = true;
                        }
                    }

                    worker.Time = worker.Time > 0 ? worker.Time - 1 : worker.Time;
                }

                time++;
            }

            return time;
        }

        private static void Main()
        {
            Console.WriteLine(StepsOrder());
            Console.WriteLine(StepsTime());
            Console.ReadKey(true);
        }
    }
}
