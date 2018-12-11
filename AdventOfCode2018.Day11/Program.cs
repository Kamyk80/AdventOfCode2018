using System;
using System.Linq;

namespace AdventOfCode2018.Day11
{
    internal static class Program
    {
        private const int Input = 7672;

        private static string LargestConstSize()
        {
            var cell = Enumerable.Range(1, 298)
                .SelectMany(y => Enumerable.Range(1, 298), (x, y) => new {x, y})
                .Select(g => new
                {
                    g.x,
                    g.y,
                    Power = Enumerable.Range(0, 3)
                        .SelectMany(y => Enumerable.Range(0, 3), (x, y) => new {x = g.x + x, y = g.y + y})
                        .Sum(c => ((c.x + 10) * c.y + Input) * (c.x + 10) / 100 % 10 - 5)
                })
                .Aggregate(new {x = 0, y = 0, Power = 0},
                    (best, next) => best.Power > next.Power ? best : next);

            return cell.x + "," + cell.y;
        }

        private static string LargestAnySize()
        {
            var cellsPower = new int[300, 300];

            for (var x = 1; x <= 300; x++)
            {
                for (var y = 1; y <= 300; y++)
                {
                    cellsPower[x - 1, y - 1] = ((x + 10) * y + Input) * (x + 10) / 100 % 10 - 5;
                }
            }

            var gridX = 1;
            var gridY = 1;
            var gridSize = 1;
            var maxPower = 0;

            for (var x = 1; x <= 300; x++)
            {
                for (var y = 1; y <= 300; y++)
                {
                    var currentPower = cellsPower[x - 1, y - 1];

                    if (currentPower > maxPower)
                    {
                        gridX = x;
                        gridY = y;
                        gridSize = 1;
                        maxPower = currentPower;
                    }

                    for (var size = 2; size <= 301 - Math.Max(x, y); size++)
                    {
                        for (var x2 = x; x2 < x + size - 1; x2++)
                        {
                            currentPower += cellsPower[x2 - 1, y + size - 2];
                        }

                        for (var y2 = y; y2 < y + size - 1; y2++)
                        {
                            currentPower += cellsPower[x + size - 2, y2 - 1];
                        }

                        currentPower += cellsPower[x + size - 2, y + size - 2];

                        if (currentPower > maxPower)
                        {
                            gridX = x;
                            gridY = y;
                            gridSize = size;
                            maxPower = currentPower;
                        }
                    }
                }
            }

            return gridX + "," + gridY + "," + gridSize;
        }

        private static void Main()
        {
            Console.WriteLine(LargestConstSize());
            Console.WriteLine(LargestAnySize());
            Console.ReadKey(true);
        }
    }
}
