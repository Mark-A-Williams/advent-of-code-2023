using System.Text.RegularExpressions;

namespace AOC2023.Net;

public class Day3
{
    public static int ExecutePart1()
    {
        var allLines = GetLinesPaddedWithDots();

        var triowiseLines = allLines.Zip(allLines.Skip(1).Zip(allLines.Skip(2)))
            .Select(o => new { Previous = o.First, Current = o.Second.First, Next = o.Second.Second });

        var total = 0;

        foreach (var trio in triowiseLines)
        {
            var current = trio.Current;
            var digitBuffer = new List<char>();
            int? numberStartIndex = null;

            for (int i = 0; i < current.Length; i++)
            {
                if (int.TryParse(current[i].ToString(), out _))
                {
                    digitBuffer.Add(current[i]);
                    numberStartIndex ??= i;
                }
                else if (digitBuffer.Count > 0)
                {
                    var windowStartIndex = Math.Max(numberStartIndex.GetValueOrDefault() - 1, 0);
                    var windowEndIndex = Math.Min(trio.Current.Length - 1, numberStartIndex.GetValueOrDefault() + digitBuffer.Count + 1);

                    var adjacentCharacters = string.Concat(trio.Previous.Substring(windowStartIndex, windowEndIndex - windowStartIndex)
                        .Concat(trio.Current.Substring(windowStartIndex, windowEndIndex - windowStartIndex))
                        .Concat(trio.Next.Substring(windowStartIndex, windowEndIndex - windowStartIndex)));

                    if (Regex.IsMatch(adjacentCharacters, @"[^0-9\\.]"))
                    {
                        var numberValue = int.Parse(string.Concat(digitBuffer));
                        total += numberValue;
                    }

                    digitBuffer.Clear();
                    numberStartIndex = null;
                }
            }
        }

        return total;
    }

    public static int ExecutePart2()
    {
        throw new NotImplementedException();
    }

    private static List<string> GetLinesPaddedWithDots()
    {
        var lines = File.ReadAllLines("../inputs/3.txt");
        var lineLength = lines.First().Length;
        var lineOfDots = string.Concat(Enumerable.Range(0, lineLength).Select(o => '.'));

        var allLines = new List<string>
        {
            lineOfDots
        };

        allLines.AddRange(lines);
        allLines.Add(lineOfDots);

        return allLines.Select(o => "." + o + ".").ToList();
    }
}
