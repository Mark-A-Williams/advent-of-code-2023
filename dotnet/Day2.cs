namespace AOC2023.Net;

public class Day2
{
    public static int ExecutePart1()
    {
        var lines = File.ReadAllLines("../inputs/2.txt");

        // This pre-filtering cut it from about 70 to 40ms. Huge.
        lines = lines.Where(line =>
        {
            var pairwiseChars = line.Zip(line.Skip(1), (a, b) => (a, b));
            return pairwiseChars.Any(o =>
            {
                if (int.TryParse(o.a.ToString(), out var tens)
                    && int.TryParse(o.b.ToString(), out var units)
                    && tens * 10 + units > 14)
                {
                    return false;
                }

                return true;
            });
        }).ToArray();

        var games = GetGamesFromInputLines(lines);

        return games.Where(o => o.IsPossible(12, 13, 14)).Sum(o => o.Id);
    }

    public static int ExecutePart2()
        => GetGamesFromInputLines(File.ReadAllLines("../inputs/2.txt"))
            .Select(game =>
            {
                var requirements = game.GetMinimumCubeRequirements();
                return requirements[Colour.Red] * requirements[Colour.Green] * requirements[Colour.Blue];
            })
            .Sum();

    private static ICollection<Game> GetGamesFromInputLines(string[] lines)
    => lines.Select(line =>
        {
            var segments = line.Split(": ");
            var id = int.Parse(segments[0].Split(" ")[1].ToString());

            var draws = segments[1]
                .Split("; ")
                .Select(draw => new Draw(draw
                    .Split(", ")
                    .Select(o =>
                    {
                        var splitEntry = o.Split(" ");
                        var count = int.Parse(splitEntry[0]);
                        var colour = (Colour)Enum.Parse(typeof(Colour), splitEntry[1], true);
                        return new { Colour = colour, Count = count };
                    })
                    .ToDictionary(
                        o => o.Colour,
                        o => o.Count)));

            return new Game(id, draws.ToList());
        }).ToList();

    private record Game(int Id, ICollection<Draw> Draws)
    {
        public bool IsPossible(int redCount, int greenCount, int blueCount)
        {
            foreach (var draw in Draws)
            {
                if (
                    draw.ColourCounts.Any(o => o.Key == Colour.Red && o.Value > redCount) ||
                    draw.ColourCounts.Any(o => o.Key == Colour.Green && o.Value > greenCount) ||
                    draw.ColourCounts.Any(o => o.Key == Colour.Blue && o.Value > blueCount))
                {
                    return false;
                }
            }

            return true;
        }

        public Dictionary<Colour, int> GetMinimumCubeRequirements()
            => Draws
                .SelectMany(d => d.ColourCounts)
                .GroupBy(o => o.Key)
                .Select(o => o.MaxBy(p => p.Value))
                .ToDictionary(o => o.Key, o => o.Value);
    }

    private record Draw(Dictionary<Colour, int> ColourCounts);
    private enum Colour { Red, Green, Blue };
}
