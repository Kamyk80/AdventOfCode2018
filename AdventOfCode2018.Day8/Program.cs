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

        private static int SumMetadata(IEnumerator<int> seq)
        {
            var nodes = seq.Read();
            var values = seq.Read();

            return
                Enumerable.Range(0, nodes)
                    .Sum(n => SumMetadata(seq))
                +
                Enumerable.Range(0, values)
                    .Sum(v => seq.Read());
        }

        private static void Main()
        {
            var input = File.ReadAllText("input.txt")
                .Split(" ")
                .Select(int.Parse);

            using (var seq = input.GetEnumerator())
            {
                Console.WriteLine(SumMetadata(seq));
            }

            Console.ReadKey(true);
        }
    }
}
