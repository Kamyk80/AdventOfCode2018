using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Day23
{
    public class Nanobot
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int R { get; set; }
    }

    internal static class Program
    {
        private static void Main()
        {
            var regex = new Regex(@"pos=<(?<x>-?\d+),(?<y>-?\d+),(?<z>-?\d+)>, r=(?<r>\d+)");
            var nanobots = File.ReadAllLines("input.txt")
                .Select(l => regex.Match(l))
                .Select(m => new Nanobot
                {
                    X = int.Parse(m.Groups["x"].Value),
                    Y = int.Parse(m.Groups["y"].Value),
                    Z = int.Parse(m.Groups["z"].Value),
                    R = int.Parse(m.Groups["r"].Value)
                })
                .ToList();

            Console.WriteLine(StrongestRange(nanobots));
            Console.WriteLine(BestDistance(nanobots)); // 60474080
            Console.ReadKey(true);
        }

        private static int StrongestRange(IList<Nanobot> nanobots)
        {
            var strongest = nanobots
                .OrderBy(n => n.R)
                .Last();

            return nanobots
                .Count(n => Math.Abs(n.X - strongest.X) + Math.Abs(n.Y - strongest.Y) + Math.Abs(n.Z - strongest.Z) <= strongest.R);
        }

        private static int BestDistance(IList<Nanobot> nanobots)
        {
            var minR = nanobots.Min(n => n.R);

            var step = 1;
            while (step * 2 < minR)
            {
                step *= 2;
            }

            var (minX, maxX) = ((nanobots.Min(n => n.X) / step - 1) * step, (nanobots.Max(n => n.X) / step + 1) * step);
            var (minY, maxY) = ((nanobots.Min(n => n.Y) / step - 1) * step, (nanobots.Max(n => n.Y) / step + 1) * step);
            var (minZ, maxZ) = ((nanobots.Min(n => n.Z) / step - 1) * step, (nanobots.Max(n => n.Z) / step + 1) * step);

            for (var s = step;; s /= 2)
            {
                var (bestX, bestY, bestZ) = (0, 0, 0);
                var bestCount = 0;

                for (var x = minX; x <= maxX; x += s)
                {
                    for (var y = minY; y <= maxY; y += s)
                    {
                        for (var z = minZ; z <= maxZ; z += s)
                        {
                            var count = nanobots
                                .Count(n => Math.Abs(n.X - x) + Math.Abs(n.Y - y) + Math.Abs(n.Z - z) <= n.R);

                            if (count > bestCount ||
                                count == bestCount && Math.Abs(x) + Math.Abs(y) + Math.Abs(z) < Math.Abs(bestX) + Math.Abs(bestY) + Math.Abs(bestZ))
                            {
                                (bestX, bestY, bestZ) = (x, y, z);
                                bestCount = count;
                            }
                        }
                    }
                }

                if (s == 1)
                {
                    return Math.Abs(bestX) + Math.Abs(bestY) + Math.Abs(bestZ);
                }

                (minX, maxX) = (bestX - s, bestX + s);
                (minY, maxY) = (bestY - s, bestY + s);
                (minZ, maxZ) = (bestZ - s, bestZ + s);
            }
        }
    }
}
