using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day3
{
    public class Claim
    {
        public int Id { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    internal static class Program
    {
        private static int Overlap()
        {
            var regex = new Regex(@"^#(\d+) @ (\d+),(\d+): (\d+)x(\d+)$");
            var claims = File.ReadAllLines("input.txt")
                .Select(l => regex.Match(l))
                .Select(m => new Claim
                {
                    Id = int.Parse(m.Groups[1].Captures[0].Value),
                    Left = int.Parse(m.Groups[2].Captures[0].Value),
                    Top = int.Parse(m.Groups[3].Captures[0].Value),
                    Width = int.Parse(m.Groups[4].Captures[0].Value),
                    Height = int.Parse(m.Groups[5].Captures[0].Value)
                })
                .ToList();

            var common = new HashSet<Tuple<int, int>>();
            var overlap = new HashSet<Tuple<int, int>>();
            var result = 0;

            foreach (var claim in claims)
            {
                for (var x = claim.Left; x < claim.Left + claim.Width; x++)
                {
                    for (var y = claim.Top; y < claim.Top + claim.Height; y++)
                    {
                        var tuple = new Tuple<int, int>(x, y);
                        if (!common.Add(tuple) && overlap.Add(tuple))
                        {
                            result++;
                        }
                    }
                }
            }

            return result;
        }

        private static void Main()
        {
            Console.WriteLine(Overlap());
            Console.ReadKey(true);
        }
    }
}
