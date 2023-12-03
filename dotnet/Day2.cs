namespace AOC2023.Net;

public class Day2
{
    public static int ExecutePart1()
    {
        var lines = File.ReadAllLines("../inputs/2.txt");

        // Possible shortcut: immediately filter out all lines which contain a number
        // greater than the largest of the numbers of available cubes
        var games = lines.Select(line =>
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
        });

        var possibleGames = games.Where(o => o.IsPossible(12, 13, 14));
        foreach (var g in possibleGames) Console.WriteLine(g);
        return possibleGames.Sum(o => o.Id);
    }

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
    }

    private record Draw(Dictionary<Colour, int> ColourCounts);
    private enum Colour { Red, Green, Blue };
}
