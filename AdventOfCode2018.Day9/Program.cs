using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Day9
{
    internal static class Program
    {
        private static LinkedListNode<T> CycleNext<T>(this LinkedListNode<T> node)
        {
            var list = node.List;
            return node.Next ?? list.First;
        }

        private static LinkedListNode<T> CyclePrevious<T>(this LinkedListNode<T> node)
        {
            var list = node.List;
            return node.Previous ?? list.Last;
        }

        private static int Winner()
        {
            var regex = new Regex(@"^(?<players>\d+) players; last marble is worth (?<points>\d+) points$");
            var match = regex.Match(File.ReadAllText("input.txt"));
            var players = int.Parse(match.Groups["players"].Value);
            var points = int.Parse(match.Groups["points"].Value);

            var board = new LinkedList<int>(new[] {0});
            var scores = new int[players];
            var current = board.First;
            var player = 0;

            for (var value = 1; value <= points; value++)
            {
                if (value % 23 > 0)
                {
                    current = current.CycleNext();
                    current = board.AddAfter(current, value);
                }
                else
                {
                    current = current.CyclePrevious().CyclePrevious().CyclePrevious().CyclePrevious().CyclePrevious().CyclePrevious();
                    scores[player] += value + current.CyclePrevious().Value;
                    board.Remove(current.CyclePrevious());
                }

                player = ++player % players;
            }

            return scores.Max(s => s);
        }

        private static void Main()
        {
            Console.WriteLine(Winner());
            Console.ReadKey(true);
        }
    }
}
