using System.Text.RegularExpressions;

namespace AOC2023.Net;

public class Day1
{
    public static int Execute()
        => File.ReadAllLines("../inputs/1.txt")
            .Select(line =>
            {
                var allNumbers = Regex
                    .Replace(line, @"[^\d]", "")
                    .Select(o => int.Parse(o.ToString()));

                var resultForLine = 10 * allNumbers.First() + allNumbers.Last();
                Console.WriteLine(resultForLine);
                return resultForLine;
            })
            .Sum();
}
