using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.Day12
{
    internal static class Program
    {
        public class Pot
        {
            public long Index { get; set; }
            public string Plant { get; set; }
        }

        private static long TwentyGenerations()
        {
            var input = File.ReadAllLines("input.txt");
            var initial = input
                .First()
                .Replace("initial state: ", "");
            var rules = input
                .Skip(2)
                .Select(r => r.Split())
                .ToDictionary(r => r[0], r => r[2]);
            var currentGen = new LinkedList<Pot>(initial
                .Select((p, i) => new Pot
                {
                    Index = i,
                    Plant = p.ToString()
                }));

            for (var gen = 1; gen <= 20; gen++)
            {
                while (string.Join("", currentGen.Select(p => p.Plant).Take(5).ToArray()) != "....#")
                {
                    currentGen.AddFirst(new Pot {Index = currentGen.First.Value.Index - 1, Plant = "."});
                }

                while (string.Join("", currentGen.Select(p => p.Plant).Reverse().Take(5).ToArray()) != "....#")
                {
                    currentGen.AddLast(new Pot {Index = currentGen.Last.Value.Index + 1, Plant = "."});
                }

                var nextGen = new LinkedList<Pot>();

                var currentPot = currentGen.First;
                while (currentPot != null)
                {
                    var currentPotConfig = (currentPot.Previous?.Previous?.Value?.Plant ?? ".") +
                                           (currentPot.Previous?.Value?.Plant ?? ".") +
                                           currentPot.Value.Plant +
                                           (currentPot.Next?.Value?.Plant ?? ".") +
                                           (currentPot.Next?.Next?.Value?.Plant ?? ".");

                    nextGen.AddLast(new Pot
                    {
                        Index = currentPot.Value.Index,
                        Plant = rules.ContainsKey(currentPotConfig) ? rules[currentPotConfig] : "."
                    });

                    currentPot = currentPot.Next;
                }

                currentGen = nextGen;

                while (currentGen.First.Value.Plant != "#")
                {
                    currentGen.RemoveFirst();
                }

                while (currentGen.Last.Value.Plant != "#")
                {
                    currentGen.RemoveLast();
                }
            }

            return currentGen.Where(p => p.Plant == "#").Sum(p => p.Index);
        }

        private static long FiftyBillionGenerations()
        {
            var input = File.ReadAllLines("input.txt");
            var initial = input
                .First()
                .Replace("initial state: ", "");
            var rules = input
                .Skip(2)
                .Select(r => r.Split())
                .ToDictionary(r => r[0], r => r[2]);
            var currentGen = new LinkedList<Pot>(initial
                .Select((p, i) => new Pot
                {
                    Index = i,
                    Plant = p.ToString()
                }));
            var nextGen = currentGen;
            var generations = 0;

            do
            {
                currentGen = nextGen;

                while (string.Join("", currentGen.Select(p => p.Plant).Take(5).ToArray()) != "....#")
                {
                    currentGen.AddFirst(new Pot { Index = currentGen.First.Value.Index - 1, Plant = "." });
                }

                while (string.Join("", currentGen.Select(p => p.Plant).Reverse().Take(5).ToArray()) != "....#")
                {
                    currentGen.AddLast(new Pot { Index = currentGen.Last.Value.Index + 1, Plant = "." });
                }

                nextGen = new LinkedList<Pot>();

                var currentPot = currentGen.First;
                while (currentPot != null)
                {
                    var currentPotConfig = (currentPot.Previous?.Previous?.Value?.Plant ?? ".") +
                                           (currentPot.Previous?.Value?.Plant ?? ".") +
                                           currentPot.Value.Plant +
                                           (currentPot.Next?.Value?.Plant ?? ".") +
                                           (currentPot.Next?.Next?.Value?.Plant ?? ".");

                    nextGen.AddLast(new Pot
                    {
                        Index = currentPot.Value.Index,
                        Plant = rules.ContainsKey(currentPotConfig) ? rules[currentPotConfig] : "."
                    });

                    currentPot = currentPot.Next;
                }

                while (currentGen.First.Value.Plant != "#")
                {
                    currentGen.RemoveFirst();
                }

                while (currentGen.Last.Value.Plant != "#")
                {
                    currentGen.RemoveLast();
                }

                while (nextGen.First.Value.Plant != "#")
                {
                    nextGen.RemoveFirst();
                }

                while (nextGen.Last.Value.Plant != "#")
                {
                    nextGen.RemoveLast();
                }

                generations++;
            } while (string.Join("", currentGen.Select(p => p.Plant).ToArray()) != string.Join("", nextGen.Select(p => p.Plant).ToArray()));

            var offset = (nextGen.First.Value.Index - currentGen.First.Value.Index) * (50000000000 - generations);

            foreach (var pot in nextGen)
            {
                pot.Index += offset;
            }

            return nextGen.Where(p => p.Plant == "#").Sum(p => p.Index);
        }

        private static void Main()
        {
            Console.WriteLine(TwentyGenerations());
            Console.WriteLine(FiftyBillionGenerations());
            Console.ReadKey(true);
        }
    }
}
