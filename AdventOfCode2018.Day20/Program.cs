using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#pragma warning disable S3358
namespace AdventOfCode2018.Day20
{
    internal static class Program
    {
        private static void Main()
        {
            var rooms = Walk(File.ReadAllText("input.txt"));

            var furthestRoom = rooms.Max(r => r.Value);
            var distantRooms = rooms.Count(r => r.Value >= 1000);

            Console.WriteLine(furthestRoom);
            Console.WriteLine(distantRooms);
            Console.ReadKey(true);
        }

        private static Dictionary<(int x, int y), int> Walk(string path)
        {
            var rooms = new Dictionary<(int x, int y), int>();
            var branches = new Stack<(int x, int y, int d)>();
            int x = 0, y = 0, d = 0;
            rooms.Add((x, y), d);

            foreach (var step in path)
            {
                if (step == 'N' || step == 'S' || step == 'E' || step == 'W')
                {
                    rooms.TryAdd((step == 'W' ? --x : step == 'E' ? ++x : x, step == 'N' ? --y : step == 'S' ? ++y : y), ++d);
                }
                else if (step == '(')
                {
                    branches.Push((x, y, d));
                }
                else if (step == '|')
                {
                    (x, y, d) = branches.Peek();
                }
                else if (step == ')')
                {
                    branches.Pop();
                }
            }

            return rooms;
        }
    }
}
#pragma warning restore S3358
