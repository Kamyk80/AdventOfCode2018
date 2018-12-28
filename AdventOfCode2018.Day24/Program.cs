using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Day24
{
    public class Group
    {
        public static int Boost { get; set; }

        public string Type { get; set; }
        public int Units { get; set; }
        public int HitPoints { get; set; }
        public IEnumerable<string> Weaknesses { get; set; } = new List<string>();
        public IEnumerable<string> Immunities { get; set; } = new List<string>();
        public int AttackDamage { get; set; }
        public string AttackType { get; set; }
        public int Initiative { get; set; }

        public int EffectivePower => Units * (AttackDamage + (Type == "Immune System" ? Boost : 0));

        public int PotentialDamage(Group enemy)
        {
            if (enemy.Weaknesses.Contains(AttackType))
            {
                return EffectivePower * 2;
            }

            if (enemy.Immunities.Contains(AttackType))
            {
                return 0;
            }

            return EffectivePower;
        }
    }

    internal static class Program
    {
        private static void Main()
        {
            var groups = File.ReadAllText("input.txt")
                .Split("\n\n")
                .SelectMany(t => ParseArmy(t
                    .TrimEnd('\n')
                    .Split('\n')))
                .ToList();

            var result = RunBattle(groups);

            Console.WriteLine(result.units);

            do
            {
                Group.Boost++;

                groups = File.ReadAllText("input.txt")
                    .Split("\n\n")
                    .SelectMany(t => ParseArmy(t
                        .TrimEnd('\n')
                        .Split('\n')))
                    .ToList();

                result = RunBattle(groups);
            } while (!result.victory);

            Console.WriteLine(result.units);
            Console.ReadKey(true);
        }

        private static IEnumerable<Group> ParseArmy(ICollection<string> lines)
        {
            var regex = new Regex(@"^(?<units>\d+) units each with (?<hitpoints>\d+) hit points (?:\((?<attributes>.+)\) )?with an attack that does (?<attackdamage>\d+) (?<attacktype>.+) damage at initiative (?<initiative>\d+)$");
            var type = lines.First().TrimEnd(':');

            return lines
                .Skip(1)
                .Select(l => regex.Match(l))
                .Select(m => ParseGroup(m, type))
                .ToList();
        }

        private static Group ParseGroup(Match match, string type)
        {
            var group = new Group
            {
                Type = type,
                Units = int.Parse(match.Groups["units"].Value),
                HitPoints = int.Parse(match.Groups["hitpoints"].Value),
                AttackDamage = int.Parse(match.Groups["attackdamage"].Value),
                AttackType = match.Groups["attacktype"].Value,
                Initiative = int.Parse(match.Groups["initiative"].Value)
            };

            return ParseAttributes(group, match.Groups["attributes"].Value);
        }

        private static Group ParseAttributes(Group group, string attributes)
        {
            foreach (var attribute in attributes.Split("; "))
            {
                if (attribute.StartsWith("weak to"))
                {
                    group.Weaknesses = attribute.Substring("weak to".Length + 1).Split(", ").ToList();
                }

                if (attribute.StartsWith("immune to"))
                {
                    group.Immunities = attribute.Substring("immune to".Length + 1).Split(", ").ToList();
                }
            }

            return group;
        }

        private static (bool victory, int units) RunBattle(List<Group> groups)
        {
            bool anyKilled;
            do
            {
                anyKilled = false;

                var attacters = groups
                    .OrderByDescending(g => g.EffectivePower)
                    .ThenByDescending(g => g.Initiative)
                    .ToList();

                var pairs = new Dictionary<Group, Group>();

                foreach (var attacker in attacters)
                {
                    var target = groups
                        .Where(g => g.Type != attacker.Type)
                        .Where(g => !pairs.ContainsValue(g))
                        .Where(g => attacker.PotentialDamage(g) > 0)
                        .OrderByDescending(attacker.PotentialDamage)
                        .ThenByDescending(g => g.EffectivePower)
                        .ThenByDescending(g => g.Initiative)
                        .FirstOrDefault();

                    if (target != null)
                    {
                        pairs.Add(attacker, target);
                    }
                }

                foreach (var pair in pairs.OrderByDescending(p => p.Key.Initiative))
                {
                    if (pair.Key.Units > 0 && pair.Value.Units > 0)
                    {
                        var killedUnits = pair.Key.PotentialDamage(pair.Value) / pair.Value.HitPoints;
                        pair.Value.Units -= killedUnits;
                        if (killedUnits > 0)
                        {
                            anyKilled = true;
                        }
                    }
                }

                groups = groups
                    .Where(g => g.Units > 0)
                    .ToList();
            } while (anyKilled);

            var victory = groups
                .All(g => g.Type == "Immune System");
            var units = groups
                .Sum(g => g.Units);

            return (victory, units);
        }
    }
}
