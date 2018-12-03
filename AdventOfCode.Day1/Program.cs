using System;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day1
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine(File.ReadAllLines("input.txt").Sum(int.Parse));

            Console.ReadKey(true);
        }
    }
}
