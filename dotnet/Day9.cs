﻿namespace AOC2023.Net;

public class Day9
{
    public static long ExecutePart1()
    {
        var lines = File.ReadAllLines("../inputs/9.txt");
        return lines.Sum(o => GetNextElementInSequence(ParseLine(o)));
    }

    public static long ExecutePart2()
    {
        throw new NotImplementedException();
    }

    private static long GetNextElementInSequence(List<long> sequence)
    {
        if (sequence.Distinct().Count() == 1)
        {
            // If all numbers in the sequence are the same, so too shall be the next one.
            return sequence[0];
        }

        var gradient = GetGradient(sequence);
        var nextGradient = GetNextElementInSequence(gradient);
        return sequence[^1] + nextGradient;
    }

    private static List<long> GetGradient(List<long> sequence)
    {
        // Linq SLOW
        List<long> result = [];
        for (int i = 0; i < sequence.Count - 1; i++)
        {
            result.Add(sequence[i + 1] - sequence[i]);
        }
        return result;
    }

    private static List<long> ParseLine(string line)
        => line.Split(' ').Select(long.Parse).ToList();
}
