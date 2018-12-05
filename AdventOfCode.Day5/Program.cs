using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Day5
{
    internal static class Program
    {
        private static int ReactedPolymer()
        {
            var polymer = new LinkedList<char>(File.ReadAllText("input.txt").Trim());
            var current = polymer.First;

            while (current.Next != null)
            {
                if (Math.Abs(current.Value - current.Next.Value) == 32)
                {
                    var next = current.Previous;
                    polymer.Remove(current.Next);
                    polymer.Remove(current);
                    current = next ?? polymer.First;
                }
                else
                {
                    current = current.Next;
                }
            }

            return polymer.Count;
        }

        private static void Main()
        {
            Console.WriteLine(ReactedPolymer());
            Console.ReadKey(true);
        }
    }
}
