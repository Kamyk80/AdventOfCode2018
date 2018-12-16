using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Day16
{
    public class Device
    {
        public static readonly string[] Instructions = {"addr", "addi", "mulr", "muli", "banr", "bani", "borr", "bori", "setr", "seti", "gtir", "gtri", "gtrr", "eqir", "eqri", "eqrr"};

        private int[] _reg;

        public void SetRegisters(int a, int b, int c, int d)
        {
            _reg = new[] { a, b, c, d };
        }

        public (int a, int b, int c, int d) GetRegisters()
        {
            return (_reg[0], _reg[1], _reg[2], _reg[3]);
        }

        public void Execute(string instruction, int a, int b, int c)
        {
            switch (instruction)
            {
                case "addr":
                    _reg[c] = _reg[a] + _reg[b];
                    break;
                case "addi":
                    _reg[c] = _reg[a] + b;
                    break;
                case "mulr":
                    _reg[c] = _reg[a] * _reg[b];
                    break;
                case "muli":
                    _reg[c] = _reg[a] * b;
                    break;
                case "banr":
                    _reg[c] = _reg[a] & _reg[b];
                    break;
                case "bani":
                    _reg[c] = _reg[a] & b;
                    break;
                case "borr":
                    _reg[c] = _reg[a] | _reg[b];
                    break;
                case "bori":
                    _reg[c] = _reg[a] | b;
                    break;
                case "setr":
                    _reg[c] = _reg[a];
                    break;
                case "seti":
                    _reg[c] = a;
                    break;
                case "gtir":
                    _reg[c] = a > _reg[b] ? 1 : 0;
                    break;
                case "gtri":
                    _reg[c] = _reg[a] > b ? 1 : 0;
                    break;
                case "gtrr":
                    _reg[c] = _reg[a] > _reg[b] ? 1 : 0;
                    break;
                case "eqir":
                    _reg[c] = a == _reg[b] ? 1 : 0;
                    break;
                case "eqri":
                    _reg[c] = _reg[a] == b ? 1 : 0;
                    break;
                case "eqrr":
                    _reg[c] = _reg[a] == _reg[b] ? 1 : 0;
                    break;
            }
        }
    }

    public class Sample
    {
        public int BeforeA { get; set; }
        public int BeforeB { get; set; }
        public int BeforeC { get; set; }
        public int BeforeD { get; set; }
        public int Opcode { get; set; }
        public int InputA { get; set; }
        public int InputB { get; set; }
        public int OutputC { get; set; }
        public int AfterA { get; set; }
        public int AfterB { get; set; }
        public int AfterC { get; set; }
        public int AfterD { get; set; }
    }

    internal static class Program
    {
        private static readonly Device Device = new Device();

        private static void Main()
        {
            var regex = new Regex(@"^Before: \[(?<beforeA>\d+), (?<beforeB>\d+), (?<beforeC>\d+), (?<beforeD>\d+)\]\n(?<opcode>\d+) (?<inputA>\d+) (?<inputB>\d+) (?<outputC>\d+)\nAfter:  \[(?<afterA>\d+), (?<afterB>\d+), (?<afterC>\d+), (?<afterD>\d+)\]$");
            var input = File.ReadAllText("input.txt").Split("\n\n\n\n");
            var samples = input[0]
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(s => regex.Match(s))
                .Select(m => new Sample
                {
                    BeforeA = int.Parse(m.Groups["beforeA"].Value),
                    BeforeB = int.Parse(m.Groups["beforeB"].Value),
                    BeforeC = int.Parse(m.Groups["beforeC"].Value),
                    BeforeD = int.Parse(m.Groups["beforeD"].Value),
                    Opcode = int.Parse(m.Groups["opcode"].Value),
                    InputA = int.Parse(m.Groups["inputA"].Value),
                    InputB = int.Parse(m.Groups["inputB"].Value),
                    OutputC = int.Parse(m.Groups["outputC"].Value),
                    AfterA = int.Parse(m.Groups["afterA"].Value),
                    AfterB = int.Parse(m.Groups["afterB"].Value),
                    AfterC = int.Parse(m.Groups["afterC"].Value),
                    AfterD = int.Parse(m.Groups["afterD"].Value)
                })
                .ToList();

            var matchingSamples = samples.Count(s => Device.Instructions.Count(i => TestSample(s, i)) > 2);

            var instructionsMap = new Dictionary<int, string>();
            while (instructionsMap.Count < Device.Instructions.Length)
            {
                var candidates = samples
                    .GroupBy(s => s.Opcode)
                    .Select(g => new
                    {
                        Opcode = g.Key,
                        Instructions = Device.Instructions
                            .Where(i => !instructionsMap.ContainsValue(i))
                            .Where(i => g.All(s => TestSample(s, i))).ToList()
                    })
                    .Where(c => c.Instructions.Count == 1)
                    .Select(c => new
                    {
                        c.Opcode,
                        Instruction = c.Instructions.First()
                    })
                    .ToList();

                foreach (var candidate in candidates)
                {
                    instructionsMap.Add(candidate.Opcode, candidate.Instruction);
                }
            }

            Device.SetRegisters(0, 0, 0, 0);

            input[1]
                .Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Split(" ")
                    .Select(int.Parse)
                    .ToList())
                .ToList()
                .ForEach(l => Device.Execute(instructionsMap[l[0]], l[1], l[2], l[3]));

            Console.WriteLine(matchingSamples);
            Console.WriteLine(Device.GetRegisters().a);
            Console.ReadKey(true);
        }

        private static bool TestSample(Sample sample, string instruction)
        {
            Device.SetRegisters(sample.BeforeA, sample.BeforeB, sample.BeforeC, sample.BeforeD);
            Device.Execute(instruction, sample.InputA, sample.InputB, sample.OutputC);
            var (a, b, c, d) = Device.GetRegisters();
            return a == sample.AfterA && b == sample.AfterB && c == sample.AfterC && d == sample.AfterD;
        }
    }
}
