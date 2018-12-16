using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.Day13
{
    public class Cart
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Direction { get; set; }
        public int NextTurn { get; set; } = -1;
    }

    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine(FirstCrash());
            Console.WriteLine(LastStanding());
            Console.ReadKey(true);
        }

        private static string FirstCrash()
        {
            var lines = File.ReadAllLines("input.txt");
            var width = lines[0].Length;
            var height = lines.Length;
            var tracks = new char[width, height];
            var carts = new List<Cart>();

            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    var spot = line[x];

                    if (spot == '^' || spot == 'v' || spot == '<' || spot == '>')
                    {
                        carts.Add(new Cart {X = x, Y = y, Direction = spot});
                        spot = spot == '^' || spot == 'v' ? '|' : '-';
                    }

                    tracks[x, y] = spot;
                }
            }

            while (true)
            {
                foreach (var cart in carts.OrderBy(c => c.Y).ThenBy(c => c.X))
                {
                    MoveCart(cart);
                    TurnCart(cart, tracks[cart.X, cart.Y]);

                    if (carts.Any(c => c != cart && c.X == cart.X && c.Y == cart.Y))
                    {
                        return cart.X + "," + cart.Y;
                    }
                }
            }
        }

        private static string LastStanding()
        {
            var lines = File.ReadAllLines("input.txt");
            var width = lines.Max(l => l.Length);
            var height = lines.Length;
            var tracks = new char[width, height];
            var carts = new List<Cart>();

            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    var spot = line[x];

                    if (spot == '^' || spot == 'v' || spot == '<' || spot == '>')
                    {
                        carts.Add(new Cart {X = x, Y = y, Direction = spot});
                        spot = spot == '^' || spot == 'v' ? '|' : '-';
                    }

                    tracks[x, y] = spot;
                }
            }

            while (carts.Count > 1)
            {
                foreach (var cart in carts.OrderBy(c => c.Y).ThenBy(c => c.X))
                {
                    MoveCart(cart);
                    TurnCart(cart, tracks[cart.X, cart.Y]);

                    var crashed = carts.FirstOrDefault(c => c != cart && c.X == cart.X && c.Y == cart.Y);
                    if (crashed != null)
                    {
                        carts.Remove(cart);
                        carts.Remove(crashed);
                    }
                }
            }

            return carts[0].X + "," + carts[0].Y;
        }

        private static void MoveCart(Cart cart)
        {
            switch (cart.Direction)
            {
                case '^':
                    cart.Y -= 1;
                    break;
                case 'v':
                    cart.Y += 1;
                    break;
                case '<':
                    cart.X -= 1;
                    break;
                case '>':
                    cart.X += 1;
                    break;
            }
        }

        private static void TurnCart(Cart cart, char track)
        {
            switch (track)
            {
                case '/' when cart.Direction == '<':
                case '/' when cart.Direction == '>':
                case '\\' when cart.Direction == '^':
                case '\\' when cart.Direction == 'v':
                    TurnLeft(cart);
                    break;
                case '/' when cart.Direction == '^':
                case '/' when cart.Direction == 'v':
                case '\\' when cart.Direction == '<':
                case '\\' when cart.Direction == '>':
                    TurnRight(cart);
                    break;
                case '+' when cart.NextTurn == -1:
                    TurnLeft(cart);
                    cart.NextTurn = 0;
                    break;
                case '+' when cart.NextTurn == 0:
                    cart.NextTurn = 1;
                    break;
                case '+' when cart.NextTurn == 1:
                    TurnRight(cart);
                    cart.NextTurn = -1;
                    break;
            }
        }

        private static void TurnLeft(Cart cart)
        {
            switch (cart.Direction)
            {
                case '^':
                    cart.Direction = '<';
                    break;
                case 'v':
                    cart.Direction = '>';
                    break;
                case '<':
                    cart.Direction = 'v';
                    break;
                case '>':
                    cart.Direction = '^';
                    break;
            }
        }

        private static void TurnRight(Cart cart)
        {
            switch (cart.Direction)
            {
                case '^':
                    cart.Direction = '>';
                    break;
                case 'v':
                    cart.Direction = '<';
                    break;
                case '<':
                    cart.Direction = '^';
                    break;
                case '>':
                    cart.Direction = 'v';
                    break;
            }
        }
    }
}
