namespace AOC2023.Net;

public class Day10
{
    public static int ExecutePart1()
    {
        var map = GetMap();
        var currentSegment = FindStart(map);
        var morePipeToCome = true;
        HashSet<PipeSegment> visitedSegments = [];

        do
        {
            var nextSegment = TryGetNextSegment(currentSegment, map, visitedSegments);
            if (nextSegment is null)
            {
                // We've run out of pipe
                morePipeToCome = false;
            }
            else
            {
                visitedSegments.Add(nextSegment);
                currentSegment = nextSegment;
            }
        }
        while (morePipeToCome);

        return visitedSegments.Count / 2;
    }

    public static int ExecutePart2()
    {
        throw new NotImplementedException();
    }

    private static PipeSegment? TryGetNextSegment(
        PipeSegment current,
        char[][] map,
        HashSet<PipeSegment> visitedSegments)
    {
        List<PipeSegment> adjacents = [];

        if (SegmentsPointingUp.Contains(current.Type))
        {
            var up = map[current.Y - 1][current.X];
            if (SegmentsPointingDown.Contains(up)) adjacents.Add(new(current.X, current.Y - 1, up));
        }

        if (SegmentsPointingDown.Contains(current.Type))
        {
            var down = map[current.Y + 1][current.X];
            if (SegmentsPointingUp.Contains(down)) adjacents.Add(new(current.X, current.Y + 1, down));
        }

        if (SegmentsPointingRight.Contains(current.Type))
        {
            var right = map[current.Y][current.X + 1];
            if (SegmentsPointingLeft.Contains(right)) adjacents.Add(new(current.X + 1, current.Y, right));
        }

        if (SegmentsPointingLeft.Contains(current.Type))
        {
            var left = map[current.Y][current.X - 1];
            if (SegmentsPointingRight.Contains(left)) adjacents.Add(new(current.X - 1, current.Y, left));
        }

        // First not Single to cover the first bit for free (where there is a choice)
        return adjacents.FirstOrDefault(c => !visitedSegments.Contains(c));
    }

    private static PipeSegment FindStart(char[][] map)
    {
        for (int x = 0; x < map[0].Length; x++)
        {
            for (int y = 0; y < map.Length; y++)
            {
                if (map[y][x] == 'S') return new(x, y, 'S');
            }
        }

        throw new ArgumentException("Map has no start");
    }

    private static char[][] GetMap()
    {
        var start = DateTimeOffset.UtcNow;
        // Cheap hack
        var mapWithoutTopAndBottomRows = File.ReadAllLines("../inputs/10.txt")
            .Select(o => $".{o}.".ToArray()).ToArray();

        var lineOfDots = Enumerable.Range(0, mapWithoutTopAndBottomRows[0].Length).Select(_ => '.').ToArray();

        List<char[]> result = [lineOfDots];
        result.AddRange(mapWithoutTopAndBottomRows);
        result.Add(lineOfDots);

        Console.WriteLine($"Parsing the map took {(DateTimeOffset.UtcNow - start).TotalMilliseconds}ms");
        return result.ToArray();
    }

    // S points in every direction
    private static readonly char[] SegmentsPointingUp = ['|', 'L', 'J', 'S'];
    private static readonly char[] SegmentsPointingDown = ['|', '7', 'F', 'S'];
    private static readonly char[] SegmentsPointingLeft = ['-', '7', 'J', 'S'];
    private static readonly char[] SegmentsPointingRight = ['-', 'L', 'F', 'S'];

    private record PipeSegment(int X, int Y, char Type);
}
