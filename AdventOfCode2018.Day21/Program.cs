using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable S4158
namespace AdventOfCode2018.Day21
{
    internal static class Program
    {
        private static void Main()
        {
            var results = new Dictionary<int, int>();
            var order = 1;

            var r1 = 0;
            do
            {
                var r4 = r1 | 65536;
                r1 = 3798839;
                do
                {
                    r1 = (r1 + (r4 & 255) & 16777215) * 65899 & 16777215;
                    if (r4 < 256) break;
                    var r5 = 0;
                    do
                    {
                        if ((r5 + 1) * 256 > r4) break;
                        r5 = r5 + 1;
                    } while (true);
                    r4 = r5;
                } while (true);
            } while (results.TryAdd(r1, order++));

            Console.WriteLine(results.OrderBy(r => r.Value).First().Key);
            Console.WriteLine(results.OrderBy(r => r.Value).Last().Key);
            Console.ReadKey(true);
        }
    }
}
#pragma warning restore S4158
