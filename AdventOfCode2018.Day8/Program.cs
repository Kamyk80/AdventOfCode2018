using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.Day8
{
    internal static class Program
    {
        private static int Read(this IEnumerator<int> seq)
        {
            seq.MoveNext();
            return seq.Current;
        }

        private static int MetadataSum(IEnumerator<int> seq)
        {
            var nodesCount = seq.Read();
            var metadataCount = seq.Read();

            return
                Enumerable.Range(0, nodesCount)
                    .Sum(n => MetadataSum(seq))
                +
                Enumerable.Range(0, metadataCount)
                    .Sum(m => seq.Read());
        }

        private static int NodeValue(IEnumerator<int> seq)
        {
            var nodesCount = seq.Read();
            var metadataCount = seq.Read();

            var nodes = Enumerable.Range(0, nodesCount)
                .Select(n => NodeValue(seq))
                .ToList();

            var metadata = Enumerable.Range(0, metadataCount)
                .Select(m => seq.Read())
                .ToList();

            return nodesCount == 0
                ? metadata
                    .Sum(m => m)
                : metadata
                    .Where(m => m > 0 && m <= nodesCount)
                    .Sum(m => nodes[m - 1]);
        }

        private static void Main()
        {
            var input = File.ReadAllText("input.txt")
                .Split(" ")
                .Select(int.Parse)
                .ToList();

            using (var seq = input.GetEnumerator())
            {
                Console.WriteLine(MetadataSum(seq));
            }

            using (var seq = input.GetEnumerator())
            {
                Console.WriteLine(NodeValue(seq));
            }

            Console.ReadKey(true);
        }
    }
}
