using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Day10
{
    internal static class Program
    {
        public class Light
        {
            public int PosX { get; set; }
            public int PosY { get; set; }
            public int VelX { get; set; }
            public int VelY { get; set; }
        }

        private static void Main()
        {
            var regex = new Regex(@"^position=<\s*(?<posx>-?\d+),\s*(?<posy>-?\d+)> velocity=<\s*(?<velx>-?\d+),\s*(?<vely>-?\d+)>$");
            var lights = File.ReadAllLines("input.txt")
                .Select(l => regex.Match(l))
                .Select(m => new Light
                {
                    PosX = int.Parse(m.Groups["posx"].Value),
                    PosY = int.Parse(m.Groups["posy"].Value),
                    VelX = int.Parse(m.Groups["velx"].Value),
                    VelY = int.Parse(m.Groups["vely"].Value)
                })
                .ToList();
            var seconds = 0;

            while (lights.Max(l => l.PosY) - lights.Min(l => l.PosY) > 9)
            {
                foreach (var light in lights)
                {
                    light.PosX += light.VelX;
                    light.PosY += light.VelY;
                }

                seconds++;
            }

            for (var y = lights.Min(l => l.PosY); y <= lights.Max(l => l.PosY); y++)
            {
                var line = new StringBuilder();
                for (var x = lights.Min(l => l.PosX); x <= lights.Max(l => l.PosX); x++)
                {
                    line.Append(lights.Any(l => l.PosX == x && l.PosY == y) ? "#" : ".");
                }
                Console.WriteLine(line);
            }

            Console.WriteLine(seconds);
            Console.ReadKey(true);
        }
    }
}
