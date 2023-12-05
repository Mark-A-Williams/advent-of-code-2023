using System.Text.RegularExpressions;

namespace AOC2023.Net;

public class Day5
{
    public static long ExecutePart1()
    {
        (var mappingSets, var seedLocations) = ParseInput();

        var lowestLocation = long.MaxValue;

        foreach (var seedLocation in seedLocations)
        {
            var currentLocation = seedLocation;
            foreach (var mapping in mappingSets)
            {
                currentLocation = mapping.MapSourceToDestination(currentLocation);
            }

            if (currentLocation < lowestLocation) lowestLocation = currentLocation;
        }

        return lowestLocation;
    }

    public static int ExecutePart2()
    {
        throw new NotImplementedException();
    }

    private static (ICollection<MappingSet> MappingSets, ICollection<long> SeedLocations) ParseInput()
    {
        var lines = File.ReadAllLines("../inputs/5.txt").Where(o => !string.IsNullOrWhiteSpace(o)).ToArray();

        var seedLocations = lines[0].Split(':')[1].Split(' ').Where(o => !string.IsNullOrWhiteSpace(o)).Select(long.Parse).ToList();

        var mappingSets = new List<MappingSet>();
        var rangeMappingsBuffer = new List<RangeMapping>();

        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            var isRangeMapping = Regex.IsMatch(line, @"[0-9]+ [0-9]+ [0-9]+");
            if (isRangeMapping)
            {
                var numbers = line.Split(' ').Select(long.Parse).ToList();
                rangeMappingsBuffer.Add(new(numbers[0], numbers[1], numbers[2]));
            }

            // We've reached a gap between mapping sets OR the end of the input
            if (rangeMappingsBuffer.Any() && (!isRangeMapping || i == lines.Length - 1))
            {
                mappingSets.Add(new(rangeMappingsBuffer.ToList()));
                rangeMappingsBuffer.Clear();
            }
        }

        return (mappingSets, seedLocations);
    }

    private record RangeMapping(long DestinationRangeStart, long SourceRangeStart, long RangeLength);

    /// <summary>
    /// E.g. Seed-to-soil map is a MappingSet containing a number of RangeMappings
    /// </summary>
    /// <param name="RangeMappings"></param>
    private record MappingSet(ICollection<RangeMapping> RangeMappings)
    {
        // Assuming they don't overlap......???????
        public long MapSourceToDestination(long source)
        {
            var relevantMapping = RangeMappings
                .SingleOrDefault(o => source >= o.SourceRangeStart && source < o.SourceRangeStart + o.RangeLength);

            if (relevantMapping is null) return source;

            return source + relevantMapping.DestinationRangeStart - relevantMapping.SourceRangeStart;
        }
    }
}
