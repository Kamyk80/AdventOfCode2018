using System;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day6
{
    internal static class Program
    {
        private static (int largestArea, int safeArea) Solution()
        {
            var points = File.ReadAllLines("input.txt")
                .Select(l => l.Split(", "))
                .Select(s => (x: int.Parse(s[0]), y: int.Parse(s[1])))
                .ToList();
            var areas = new int[points.Count];
            var safeArea = 0;

            var minX = points.Min(p => p.x);
            var maxX = points.Max(p => p.x);
            var minY = points.Min(p => p.y);
            var maxY = points.Max(p => p.y);

            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y <= maxY; y++)
                {
                    var dists = points
                        .Select((p, i) => new {Point = i, Dist = Math.Abs(x - p.x) + Math.Abs(y - p.y)})
                        .ToList();

                    safeArea += dists.Sum(p => p.Dist) < 10000 ? 1 : 0;

                    var closest = dists.GroupBy(p => p.Dist)
                        .OrderBy(g => g.Key)
                        .First();

                    if (closest.Count() == 1)
                    {
                        var point = closest.First().Point;

                        areas[point] = x == minX || x == maxX || y == minY || y == maxY ? -1 : areas[point] + 1;
                    }
                }
            }

            return (largestArea: areas.Max(a => a), safeArea: safeArea);
        }

        private static void Main()
        {
            var (largestArea, safeArea) = Solution();
            Console.WriteLine(largestArea);
            Console.WriteLine(safeArea);
            Console.ReadKey(true);
        }
    }
}
