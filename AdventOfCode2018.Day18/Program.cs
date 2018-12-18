using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2018.Day18
{
    internal static class Program
    {
        private static void Main()
        {
            var map = File.ReadAllLines("input.txt");

            Console.WriteLine(RunMap(map, 10));
            Console.WriteLine(RunMap(map, 1000000000));
            Console.ReadKey(true);
        }

        private static int RunMap(string[] map, int minutes)
        {
            var height = map.Length;
            var width = map[0].Length;
            var results = new List<string> {string.Join("\n", map)};
            var index = -1;

            for (var minute = 1; minute <= minutes; minute++)
            {
                var next = new string[map.Length];

                for (var y = 0; y < height; y++)
                {
                    var sb = new StringBuilder();

                    for (var x = 0; x < width; x++)
                    {
                        var trees = 0;
                        var lumber = 0;

                        for (var dx = (x > 0 ? x - 1 : x); dx <= (x < width - 1 ? x + 1 : x); dx++)
                        {
                            for (var dy = (y > 0 ? y - 1 : y); dy <= (y < height - 1 ? y + 1 : y); dy++)
                            {
                                if (dx == x && dy == y)
                                {
                                    continue;
                                }

                                if (map[dy][dx] == '|')
                                {
                                    trees++;
                                }

                                if (map[dy][dx] == '#')
                                {
                                    lumber++;
                                }
                            }
                        }

                        switch (map[y][x])
                        {
                            case '.' when trees >= 3:
                                sb.Append('|');
                                break;
                            case '|' when lumber >= 3:
                                sb.Append('#');
                                break;
                            case '#' when trees == 0 || lumber == 0:
                                sb.Append('.');
                                break;
                            default:
                                sb.Append(map[y][x]);
                                break;
                        }
                    }

                    next[y] = sb.ToString();
                }

                map = next;

                var result = string.Join('\n', map);

                if (results.Contains(result))
                {
                    index = results.IndexOf(result);
                    break;
                }

                results.Add(result);
            }

            if (index > -1)
            {
                index += (minutes - results.Count) % (results.Count - index);
                map = results[index].Split('\n');
            }

            var woods = map
                .Sum(l => l
                    .Count(c => c == '|'));
            var lumberyards = map
                .Sum(l => l
                    .Count(c => c == '#'));

            return woods * lumberyards;
        }
    }
}
