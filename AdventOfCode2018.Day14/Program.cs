using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Day14
{
    internal static class Program
    {
        private const string Input = "704321";

        private static void Main()
        {
            Console.WriteLine(RecipeScores());
            Console.WriteLine(RecipesCount());
            Console.ReadKey(true);
        }

        private static string RecipeScores()
        {
            var recipes = new List<int>(new[] {3, 7});
            var firstElf = 0;
            var secondElf = 1;

            var input = int.Parse(Input);
            while (recipes.Count < input + 10)
            {
                var sum = recipes[firstElf] + recipes[secondElf];
                if (sum >= 10)
                {
                    recipes.Add(1);
                    sum -= 10;
                }

                recipes.Add(sum);

                firstElf = (firstElf + 1 + recipes[firstElf]) % recipes.Count;
                secondElf = (secondElf + 1 + recipes[secondElf]) % recipes.Count;
            }

            return string.Join("", recipes.Skip(input).Take(10));
        }

        private static int RecipesCount()
        {
            var recipes = new List<int>(new[] {3, 7});
            var firstElf = 0;
            var secondElf = 1;
            var input = Input.Select(i => i - '0').ToList();

            while (true)
            {
                var sum = recipes[firstElf] + recipes[secondElf];

                if (sum >= 10)
                {
                    recipes.Add(1);
                    sum -= 10;
                }

                recipes.Add(sum);

                firstElf = (firstElf + 1 + recipes[firstElf]) % recipes.Count;
                secondElf = (secondElf + 1 + recipes[secondElf]) % recipes.Count;

                var count = FindSequence(recipes, input);
                if (count > -1)
                {
                    return count;
                }
            }
        }


        private static int FindSequence(IReadOnlyList<int> recipes, IReadOnlyList<int> input)
        {
            if (recipes.Count <= input.Count)
            {
                return -1;
            }

            var count = -1;

            if (recipes[recipes.Count - input.Count - 1] == input[0])
            {
                count = recipes.Count - input.Count - 1;
            }

            if (recipes[recipes.Count - input.Count] == input[0])
            {
                count = recipes.Count - input.Count;
            }

            if (count == -1)
            {
                return -1;
            }

            for (var i = 1; i < input.Count; i++)
            {
                if (recipes[count + i] != input[i])
                {
                    return -1;
                }
            }

            return count;
        }
    }
}
