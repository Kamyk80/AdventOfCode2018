using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.Day25
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int T { get; set; }
    }

    internal static class Program
    {
        private static void Main()
        {
            var points = File.ReadAllLines("input.txt")
                .Select(l => l.Split(','))
                .Select(s => new Point
                {
                    X = int.Parse(s[0]),
                    Y = int.Parse(s[1]),
                    Z = int.Parse(s[2]),
                    T = int.Parse(s[3])
                })
                .ToList();
            var constellations = 0;

            while (points.Any())
            {
                var constellation = new List<Point> {points[0]};
                points.RemoveAt(0);

                for (var i = 0; i < constellation.Count; i++)
                {
                    for (var j = 0; j < points.Count; j++)
                    {
                        if (Distance(constellation[i], points[j]) <= 3)
                        {
                            constellation.Add(points[j]);
                            points.RemoveAt(j--);
                        }
                    }
                }

                constellations++;
            }

            Console.WriteLine(constellations);
            Console.ReadKey(true);
        }

        private static int Distance(Point p1, Point p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y) + Math.Abs(p1.Z - p2.Z) + Math.Abs(p1.T - p2.T);
        }
    }
}
