using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Day17
{
    internal static class Program
    {
        private static StringBuilder[] _map;
        private static int _minY;
        private static int _maxY;

        private static void Main()
        {
            ParseMap("input.txt");

            RunMap();

            var waterReach = _map
                .Skip(_minY + 1)
                .Select(l => l.ToString())
                .Sum(l => l.Count(c => c == '|' || c == '~'));
            var waterLeft = _map
                .Skip(_minY - 1)
                .Select(l => l.ToString())
                .Sum(l => l.Count(c => c == '~'));

            Console.WriteLine(waterReach);
            Console.WriteLine(waterLeft);
            Console.ReadKey(true);
        }

        private static void RunMap()
        {
            var y = 1;
            while (y < _maxY)
            {
                var restLevel = false;

                for (var x = 0; x < _map[y].Length; x++)
                {
                    var restCurrent = false;

                    if ((_map[y-1][x] == '+' || _map[y-1][x] == '|') && (_map[y][x] == '.' || _map[y][x] == '|'))
                    {
                        _map[y][x] = '|';

                        if (_map[y+1][x] == '#' || _map[y+1][x] == '~')
                        {
                            var left = x;
                            while ((_map[y][left-1] == '.' || _map[y][left-1] == '|') && (_map[y+1][left-1] == '#' || _map[y+1][left-1] == '~'))
                            {
                                left--;
                            }

                            var right = x;
                            while ((_map[y][right+1] == '.' || _map[y][right+1] == '|') && (_map[y+1][right+1] == '#' || _map[y+1][right+1] == '~'))
                            {
                                right++;
                            }

                            if (_map[y][left-1] == '#' && _map[y][right+1] == '#')
                            {
                                restLevel = true;
                                restCurrent = true;
                            }

                            for (var i = left; i <= right; i++)
                            {
                                _map[y][i] = restCurrent ? '~' : '|';
                            }

                            if (_map[y][left-1] == '.')
                            {
                                _map[y][left-1] = '|';
                            }

                            if (_map[y][right+1] == '.')
                            {
                                _map[y][right+1] = '|';
                            }
                        }
                    }
                }

                y = restLevel ? y - 1 : y + 1;
            }
        }

        private static void ParseMap(string path)
        {
            var regex = new Regex(@"^(?<c1l>x|y)=(?<c1v>\d+), (?<c2l>x|y)=(?<c2f>\d+)..(?<c2t>\d+)$");
            var veins = File.ReadAllLines(path)
                .Select(l => regex.Match(l))
                .Select(m => new
                {
                    c1l = m.Groups["c1l"].Value[0],
                    c1v = int.Parse(m.Groups["c1v"].Value),
                    c2l = m.Groups["c2l"].Value[0],
                    c2f = int.Parse(m.Groups["c2f"].Value),
                    c2t = int.Parse(m.Groups["c2t"].Value)
                })
                .ToList();
            var minX = veins.Min(c => c.c1l == 'x' ? c.c1v : c.c2f) - 1;
            var maxX = veins.Max(c => c.c1l == 'x' ? c.c1v : c.c2t) + 1;
            _minY = veins.Min(c => c.c1l == 'y' ? c.c1v : c.c2f) - 1;
            _maxY = veins.Max(c => c.c1l == 'y' ? c.c1v : c.c2t) + 1;
            _map = new StringBuilder[_maxY+1];

            for (var y = 0; y < _maxY + 1; y++)
            {
                _map[y] = new StringBuilder();
                _map[y].Append('.', maxX - minX + 1);
            }

            _map[0][500 - minX] = '+';

            foreach (var vein in veins)
            {
                for (var c2 = vein.c2f; c2 <= vein.c2t; c2++)
                {
                    if (vein.c1l == 'x')
                    {
                        _map[c2][vein.c1v - minX] = '#';
                    }
                    else
                    {
                        _map[vein.c1v][c2 - minX] = '#';
                    }
                }
            }
        }
    }
}
