using System;
using System.IO;

namespace AdventOfCode2018.Day15
{
    internal static class Program
    {
        private static void Main()
        {
            var map = File.ReadAllLines("input.txt");

            var firstResult = new Game(map, 3).Run();
            Console.WriteLine(firstResult);

            for (var power = 4;; power++)
            {
                var nextResult = new Game(map, power).Run();
                if (nextResult > 0)
                {
                    Console.WriteLine(nextResult);
                    break;
                }
            }

            Console.ReadKey(true);
        }
    }
}
