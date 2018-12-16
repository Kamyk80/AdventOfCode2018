using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2018.Day15
{
    public class Game
    {
        private static readonly (int dx, int dy)[] Offsets = {(0, -1), (-1, 0), (1, 0), (0, 1)};

        private readonly string[] _map;
        private List<Unit> _units = new List<Unit>();
        private readonly int _power;

        public Game(string[] map, int power)
        {
            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[0].Length; x++)
                {
                    if (map[y][x] == 'G')
                    {
                        _units.Add(new Goblin {X = x, Y = y});
                    }

                    if (map[y][x] == 'E')
                    {
                        _units.Add(new Elf {X = x, Y = y});
                    }
                }
            }

            _map = map
                .Select(l => l
                    .Replace('G', '.')
                    .Replace('E', '.'))
                .ToArray();
            _power = power;
        }

        public int Run()
        {
            for (var rounds = 0;; rounds++)
            {
                _units = _units
                    .OrderBy(u => u.Y)
                    .ThenBy(u => u.X)
                    .ToList();

                for (var i = 0; i < _units.Count; i++)
                {
                    var unit = _units[i];
                    var targets = _units
                        .Where(t => t.Type != unit.Type)
                        .ToList();

                    if (!targets.Any())
                    {
                        return rounds * _units.Sum(u => u.Health);
                    }

                    if (!targets.Any(t => IsAdjacent(t, unit)))
                    {
                        TryMove(unit, targets);
                    }

                    var bestTarget = targets
                        .Where(t => IsAdjacent(t, unit))
                        .OrderBy(t => t.Health)
                        .ThenBy(t => t.Y)
                        .ThenBy(t => t.X)
                        .FirstOrDefault();

                    if (bestTarget != null)
                    {
                        bestTarget.Health -= unit is Elf ? _power : 3;

                        if (bestTarget.Health < 1)
                        {
                            if (_power > 3 && bestTarget is Elf)
                            {
                                return 0;
                            }

                            var index = _units.IndexOf(bestTarget);
                            _units.RemoveAt(index);
                            if (index < i)
                            {
                                i--;
                            }
                        }
                    }
                }
            }
        }

        private void TryMove(Unit unit, IEnumerable<Unit> targets)
        {
            var inRange = new HashSet<(int x, int y)>();

            foreach (var target in targets)
            {
                var destinations = Offsets
                    .Select(o => (x: target.X + o.dx, y: target.Y + o.dy))
                    .Where(d => IsOpen(d.x, d.y))
                    .ToList();

                foreach (var destination in destinations)
                {
                    inRange.Add(destination);
                }
            }

            var nodes = new Queue<(int x, int y)>();
            var parents = new Dictionary<(int x, int y), (int x, int y)>();

            nodes.Enqueue((unit.X, unit.Y));
            parents.Add((unit.X, unit.Y), (-1, -1));

            while (nodes.Count > 0)
            {
                var (x, y) = nodes.Dequeue();
                var destinations = Offsets
                    .Select(o => (x: x + o.dx, y: y + o.dy))
                    .Where(d => !parents.ContainsKey(d))
                    .Where(d => IsOpen(d.x, d.y))
                    .ToList();

                foreach (var destination in destinations)
                {
                    nodes.Enqueue(destination);
                    parents.Add(destination, (x, y));
                }
            }

            List<(int x, int y)> GetPath(int destX, int destY)
            {
                if (!parents.ContainsKey((destX, destY)))
                {
                    return null;
                }

                var path = new List<(int x, int y)>();
                var (curX, curY) = (destX, destY);

                while (curX != unit.X || curY != unit.Y)
                {
                    path.Add((curX, curY));
                    (curX, curY) = parents[(curX, curY)];
                }

                path.Reverse();
                return path;
            }

            var bestPath = inRange
                .Select(d => (x: d.x, y: d.y, path: GetPath(d.x, d.y)))
                .Where(d => d.path != null)
                .OrderBy(d => d.path.Count)
                .ThenBy(d => d.y)
                .ThenBy(d => d.x)
                .Select(d => d.path)
                .FirstOrDefault();

            if (bestPath != null)
            {
                (unit.X, unit.Y) = bestPath[0];
            }
        }

        private static bool IsAdjacent(Unit u1, Unit u2) => Math.Abs(u1.X - u2.X) + Math.Abs(u1.Y - u2.Y) == 1;

        private bool IsOpen(int x, int y) => _map[y][x] == '.' && _units.All(u => u.X != x || u.Y != y);

#pragma warning disable S1144 // Unused private types or members should be removed
        // ReSharper disable once UnusedMember.Local
        private void DrawMap()
        {
            for (var y = 0; y < _map.Length; y++)
            {
                var sb = new StringBuilder();

                for (var x = 0; x < _map[0].Length; x++)
                {
                    sb.Append(_units.FirstOrDefault(u => u.X == x && u.Y == y)?.Type ?? _map[y][x]);
                }

                sb.Append("   ");
                sb.Append(string.Join(", ", _units
                    .Where(u => u.Y == y)
                    .OrderBy(u => u.X)
                    .Select(u => $"{u.Type}({u.Health})")
                    .ToList()));

                Console.WriteLine(sb);
            }
        }
#pragma warning restore S1144 // Unused private types or members should be removed
    }
}
