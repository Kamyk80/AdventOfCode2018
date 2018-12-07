using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Day7
{
    internal static class Program
    {
        private static void Main()
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

            while (!steps.All(s => s.Value))
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

            Console.WriteLine(order);
            Console.ReadKey(true);
        }
    }
}
