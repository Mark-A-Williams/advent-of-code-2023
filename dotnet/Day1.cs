using System.Text.RegularExpressions;

namespace AOC2023.Net;

public class Day1
{
    public static int ExecutePart1()
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

    public static int ExecutePart2()
        => File.ReadAllLines("../inputs/1.txt")
            .Select(line =>
            {
                var firstNumber = GetEndNumberOfLine(line, false);
                var lastNumber = GetEndNumberOfLine(line, true);
                return 10 * firstNumber.GetValueOrDefault() + lastNumber.GetValueOrDefault();
            }).Sum();

    private static Dictionary<string, int?> NumberMappings = new()
    {
        {"zero", 0},
        {"one", 1},
        {"two", 2},
        {"three", 3},
        {"four", 4},
        {"five", 5},
        {"six", 6},
        {"seven", 7},
        {"eight", 8},
        {"nine", 9}
    };

    private static int? GetEndNumberOfLine(string line, bool last)
    {
        var chars = line.Select((character, index) => (character, index));

        if (last)
        {
            chars = chars.OrderByDescending(o => o.index);
        }

        foreach (var (character, index) in chars)
        {
            if (int.TryParse(character.ToString(), out var number))
            {
                return number;
            }

            var matchingNumber = NumberMappings
                .SingleOrDefault(o => line.Substring(index).StartsWith(o.Key));

            if (matchingNumber.Value is not null)
            {
                return matchingNumber.Value.Value;
            }
        }

        return null;
    }
}
