using System.Text.RegularExpressions;

namespace AOC2023.Net;

public class Day5
{
    public static long ExecutePart1()
    {
        var mappingSets = ParseMappingSetsFromInput();
        var seedLocations = ParseSeedLocationsFromInput();

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

    public static long ExecutePart2()
    {
        var mappingSets = ParseMappingSetsFromInput();
        var seedRanges = ParseSeedRangesFromInput();

        var lowestLocation = long.MaxValue;
        var totalSeedLocationCount = seedRanges.Sum(o => o.Length);
        long locationsSearched = 0;

        foreach (var seedRange in seedRanges)
        {
            Console.WriteLine($"Beginning search for seed range with start index {seedRange.Start}");
            for (var i = 0; i < seedRange.Length; i++)
            {
                var seedLocation = seedRange.Start + i;
                var currentLocation = seedLocation;

                foreach (var mapping in mappingSets)
                {
                    currentLocation = mapping.MapSourceToDestination(currentLocation);
                }

                if (currentLocation < lowestLocation) lowestLocation = currentLocation;

                locationsSearched++;
                if (locationsSearched % 10_000_000 == 0)
                {
                    Console.WriteLine($"Total progress (locations searched): {(decimal)locationsSearched / totalSeedLocationCount * 100}%");
                }
            }
        }

        return lowestLocation;
    }

    private static ICollection<MappingSet> ParseMappingSetsFromInput()
    {
        var lines = File.ReadAllLines("../inputs/5.txt").Where(o => !string.IsNullOrWhiteSpace(o)).ToArray();

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

        return mappingSets;
    }

    private static ICollection<long> ParseSeedLocationsFromInput()
        => File.ReadAllLines("../inputs/5.txt")[0]
            .Split(':')[1]
            .Split(' ')
            .Where(o => !string.IsNullOrWhiteSpace(o))
            .Select(long.Parse)
            .ToList();

    private static ICollection<(long Start, long Length)> ParseSeedRangesFromInput()
        => ParseSeedLocationsFromInput()
            .Chunk(2)
            .Select(o =>
            {
                var chunkContent = o.ToArray();
                return (chunkContent[0], chunkContent[1]);
            })
            .ToList();

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
