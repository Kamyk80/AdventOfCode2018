using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.Day05
{
    internal static class Program
    {
        private static int ReactedPolymer(string text)
        {
            var polymer = new LinkedList<char>(text);
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

        private static int ImprovedPolymer(string text)
        {
            return text
                .ToLower()
                .Distinct()
                .Select(c => text.Replace(c.ToString(), "").Replace(c.ToString().ToUpper(), ""))
                .Min(ReactedPolymer);
        }

        private static void Main()
        {
            var text = File.ReadAllText("input.txt").Trim();

            Console.WriteLine(ReactedPolymer(text));
            Console.WriteLine(ImprovedPolymer(text));
            Console.ReadKey(true);
        }
    }
}
