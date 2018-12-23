using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Day22
{
    public class Region
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Type { get; set; }
        public int Torch { get; set; } = int.MaxValue;
        public int Gear { get; set; } = int.MaxValue;
        public int Neither { get; set; } = int.MaxValue;
    }

    internal static class Program
    {
        private static readonly (int dx, int dy)[] Offsets = {(0, -1), (-1, 0), (1, 0), (0, 1)};

        private static void Main()
        {
            var regex = new Regex(@"depth: (?<depth>\d+)\ntarget: (?<targetX>\d+),(?<targetY>\d+)");
            var match = regex.Match(File.ReadAllText("input.txt"));

            var depth = int.Parse(match.Groups["depth"].Value);
            var targetX = int.Parse(match.Groups["targetX"].Value);
            var targetY = int.Parse(match.Groups["targetY"].Value);

            var cave = CalculateCave(depth, targetX, targetY, 1000, 1000);

            Console.WriteLine(RiskLevel(cave, targetX, targetY));
            Console.WriteLine(BestTime(cave, targetX, targetY));
            Console.ReadKey(true);
        }

        private static Region[,] CalculateCave(int depth, int targetX, int targetY, int maxX, int maxY)
        {
            var erosion = new int[maxX + 1, maxY + 1];
            var cave = new Region[maxX + 1, maxY + 1];

            for (var y = 0; y <= maxY; y++)
            {
                for (var x = 0; x <= maxX; x++)
                {
                    int index;
                    if (x == 0 && y == 0 || x == targetX && y == targetY)
                    {
                        index = 0;
                    }
                    else if (y == 0)
                    {
                        index = x * 16807;
                    }
                    else if (x == 0)
                    {
                        index = y * 48271;
                    }
                    else
                    {
                        index = erosion[x - 1, y] * erosion[x, y - 1];
                    }

                    erosion[x, y] = (index + depth) % 20183;
                    cave[x, y] = new Region {X = x, Y = y, Type = erosion[x, y] % 3};
                }
            }

            return cave;
        }

        private static int RiskLevel(Region[,] cave, int targetX, int targetY)
        {
            var risk = 0;
            for (var y = 0; y <= targetY; y++)
            {
                for (var x = 0; x <= targetX; x++)
                {
                    risk += cave[x, y].Type;
                }
            }

            return risk;
        }

        private static int BestTime(Region[,] cave, int targetX, int targetY)
        {
            var nodes = Enumerable.Range(0, 8).ToDictionary(i => i, i => new Queue<Region>());

            cave[0, 0].Torch = 0;
            cave[0, 0].Neither = 7;

            nodes[0].Enqueue(cave[0, 0]);
            nodes[7].Enqueue(cave[0, 0]);

            var time = 0;

            while (true)
            {
                nodes.Add(time + 8, new Queue<Region>());

                var queue = nodes[time];

                while (queue.Count > 0)
                {
                    var region = queue.Dequeue();

                    if (region.X == targetX && region.Y == targetY && region.Torch == time)
                    {
                        return time;
                    }

                    if (region.Torch == time || region.Gear == time || region.Neither == time)
                    {
                        var destinations = Offsets
                            .Select(o => (x: region.X + o.dx, y: region.Y + o.dy))
                            .Where(d => d.x >= 0 && d.y >= 0)
                            .Select(d => cave[d.x, d.y])
                            .ToList();

                        foreach (var destination in destinations)
                        {
                            if (region.Torch == time)
                            {
                                if (destination.Type == 0)
                                {
                                    if (destination.Torch > region.Torch + 1)
                                    {
                                        destination.Torch = region.Torch + 1;
                                        nodes[destination.Torch].Enqueue(destination);
                                    }

                                    if (destination.Gear > region.Torch + 8)
                                    {
                                        destination.Gear = region.Torch + 8;
                                        nodes[destination.Gear].Enqueue(destination);
                                    }
                                }

                                if (destination.Type == 2)
                                {
                                    if (destination.Torch > region.Torch + 1)
                                    {
                                        destination.Torch = region.Torch + 1;
                                        nodes[destination.Torch].Enqueue(destination);
                                    }

                                    if (destination.Neither > region.Torch + 8)
                                    {
                                        destination.Neither = region.Torch + 8;
                                        nodes[destination.Neither].Enqueue(destination);
                                    }
                                }
                            }

                            if (region.Gear == time)
                            {
                                if (destination.Type == 0)
                                {
                                    if (destination.Gear > region.Gear + 1)
                                    {
                                        destination.Gear = region.Gear + 1;
                                        nodes[destination.Gear].Enqueue(destination);
                                    }

                                    if (destination.Torch > region.Gear + 8)
                                    {
                                        destination.Torch = region.Gear + 8;
                                        nodes[destination.Torch].Enqueue(destination);
                                    }
                                }

                                if (destination.Type == 1)
                                {
                                    if (destination.Gear > region.Gear + 1)
                                    {
                                        destination.Gear = region.Gear + 1;
                                        nodes[destination.Gear].Enqueue(destination);
                                    }

                                    if (destination.Neither > region.Gear + 8)
                                    {
                                        destination.Neither = region.Gear + 8;
                                        nodes[destination.Neither].Enqueue(destination);
                                    }
                                }
                            }

                            if (region.Neither == time)
                            {
                                if (destination.Type == 1)
                                {
                                    if (destination.Neither > region.Neither + 1)
                                    {
                                        destination.Neither = region.Neither + 1;
                                        nodes[destination.Neither].Enqueue(destination);
                                    }

                                    if (destination.Gear > region.Neither + 8)
                                    {
                                        destination.Gear = region.Neither + 8;
                                        nodes[destination.Gear].Enqueue(destination);
                                    }
                                }

                                if (destination.Type == 2)
                                {
                                    if (destination.Neither > region.Neither + 1)
                                    {
                                        destination.Neither = region.Neither + 1;
                                        nodes[destination.Neither].Enqueue(destination);
                                    }

                                    if (destination.Torch > region.Neither + 8)
                                    {
                                        destination.Torch = region.Neither + 8;
                                        nodes[destination.Torch].Enqueue(destination);
                                    }
                                }
                            }
                        }
                    }
                }

                time++;
            }
        }
    }
}
