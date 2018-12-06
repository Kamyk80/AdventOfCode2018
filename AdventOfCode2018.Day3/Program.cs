using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Day3
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
        private static int ClaimsOverlap()
        {
            var regex = new Regex(@"^#(\d+) @ (\d+),(\d+): (\d+)x(\d+)$");
            return File.ReadAllLines("input.txt")
                .Select(l => regex.Match(l))
                .Select(m => new Claim
                {
                    Id = int.Parse(m.Groups[1].Value),
                    Left = int.Parse(m.Groups[2].Value),
                    Top = int.Parse(m.Groups[3].Value),
                    Width = int.Parse(m.Groups[4].Value),
                    Height = int.Parse(m.Groups[5].Value)
                })
                .SelectMany(c => Enumerable.Range(c.Left, c.Width), (c, x) => new {c.Top, c.Height, x})
                .SelectMany(c => Enumerable.Range(c.Top, c.Height), (c, y) => new {c.x, y})
                .GroupBy(c => c)
                .Count(g => g.Count() > 1);
        }

        private static int ClaimIntact()
        {
            var regex = new Regex(@"^#(\d+) @ (\d+),(\d+): (\d+)x(\d+)$");
            var claims = File.ReadAllLines("input.txt")
                .Select(l => regex.Match(l))
                .Select(m => new Claim
                {
                    Id = int.Parse(m.Groups[1].Value),
                    Left = int.Parse(m.Groups[2].Value),
                    Top = int.Parse(m.Groups[3].Value),
                    Width = int.Parse(m.Groups[4].Value),
                    Height = int.Parse(m.Groups[5].Value)
                })
                .ToList();

            return claims
                .First(c1 => claims
                    .Where(c2 => c1 != c2)
                    .All(c2 => !AreOverlapping(c1, c2)))
                .Id;
        }

        private static bool AreOverlapping(Claim c1, Claim c2)
        {
            return c1.Left < c2.Left + c2.Width &&
                   c1.Left + c1.Width > c2.Left &&
                   c1.Top < c2.Top + c2.Height &&
                   c1.Height + c1.Top > c2.Top;
        }

        private static void Main()
        {
            Console.WriteLine(ClaimsOverlap());
            Console.WriteLine(ClaimIntact());
            Console.ReadKey(true);
        }
    }
}
