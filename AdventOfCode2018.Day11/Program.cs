using System;
using System.Linq;

namespace AdventOfCode2018.Day11
{
    internal static class Program
    {
        private const int Input = 7672;

        private static void Main()
        {
            var cell = Enumerable.Range(1, 298)
                .SelectMany(x => Enumerable.Range(1, 298)
                    .Select(y => new {x, y}))
                .Select(c1 => new
                {
                    c1.x,
                    c1.y,
                    Power = Enumerable.Range(0, 3)
                        .SelectMany(x => Enumerable.Range(0, 3)
                            .Select(y => new {x = c1.x + x, y = c1.y + y}))
                        .Sum(c2 => ((c2.x + 10) * c2.y + Input) * (c2.x + 10) / 100 % 10 - 5)
                })
                .Aggregate(new { x = 0, y = 0, Power = 0 },
                    (best, next) => best.Power > next.Power ? best : next);

            Console.WriteLine(cell.x + "," + cell.y);
            Console.ReadKey(true);
        }
    }
}
