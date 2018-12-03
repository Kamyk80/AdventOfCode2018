using System;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day2
{
    internal static class Program
    {
        private static int Checksum()
        {
            var lines = File.ReadAllLines("input.txt");
            var twos = 0;
            var threes = 0;

            foreach (var line in lines)
            {
                var groups = line.GroupBy(c => c).ToList();
                if (groups.Any(g => g.Count() == 2))
                {
                    twos++;
                }
                if (groups.Any(g => g.Count() == 3))
                {
                    threes++;
                }
            }

            return twos * threes;
        }

        private static void Main()
        {
            Console.WriteLine(Checksum());
            Console.ReadKey(true);
        }
    }
}
