using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.Day19
{
    public class Device
    {
        private readonly int[] _reg = new int[6];
        private readonly int _index;
        private int _pointer;

        public Device(int index)
        {
            _index = index;
            _reg[0] = 0;
        }

        public long Run(List<string> program)
        {
            while (_pointer < program.Count)
            {
                _reg[_index] = _pointer;
                var line = program[_pointer].Split(' ');
                Execute(line[0], int.Parse(line[1]), int.Parse(line[2]), int.Parse(line[3]));
                _pointer = _reg[_index] + 1;
            }

            return _reg[0];
        }

        private void Execute(string instruction, int a, int b, int c)
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

    internal static class Program
    {
        private static void Main()
        {
            var lines = File.ReadAllLines("input.txt");
            var index = int.Parse(lines[0].Split(' ')[1]);
            var program = lines.Skip(1).ToList();

            Console.WriteLine(new Device(index).Run(program));

            var r0 = 0;
            var r1 = 1;
            var r2 = 10551428;

            do
            {
                if (r2 % r1 == 0)
                {
                    r0 += r1;
                }

                r1++;
            } while (r1 <= r2);

            Console.WriteLine(r0);
            Console.ReadKey(true);
        }
    }
}
