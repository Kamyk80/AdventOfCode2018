using System;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2018.Day02
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

        private static string Prototype()
        {
            var lines = File.ReadAllLines("input.txt");

            return lines
                .SelectMany(l => lines, Compare)
                .Where(c => c.diff == 1)
                .Select(c => c.common)
                .First();
        }

        private static (int diff, string common) Compare(string line1, string line2)
        {
            var common = new StringBuilder();
            var diff = 0;

            for (var i = 0; i < new[] {line1.Length, line2.Length}.Min(); i++)
            {
                if (line1[i] == line2[i])
                {
                    common.Append(line1[i]);
                }
                else
                {
                    diff++;
                }
            }

            return (diff, common.ToString());
        }

        private static void Main()
        {
            Console.WriteLine(Checksum());
            Console.WriteLine(Prototype());
            Console.ReadKey(true);
        }
    }
}
