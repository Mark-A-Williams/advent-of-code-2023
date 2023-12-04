namespace AOC2023.Net;

public class Day4
{
    public static int ExecutePart1()
        => File.ReadAllLines("../inputs/4.txt")
            .Select(ParseCardFromLine)
            .ToArray()
            .Sum(card => card.Score);

    public static int ExecutePart2()
    {
        var allCards = File.ReadAllLines("../inputs/4.txt")
            .Select(ParseCardFromLine)
            .ToList();

        var highestId = allCards.Max(card => card.Id);

        for (int i = 1; i <= highestId; i++)
        {
            var countOfThisCard = allCards.Count(c => c.Id == i);
            var card = allCards.First(c => c.Id == i);

            var cardsToClone = allCards.Where(c => c.Id > i && c.Id <= i + card.MatchingNumbersCount)
                .GroupBy(c => c.Id)
                .Select(grouping => grouping.First())
                .ToList();

            for (int j = 0; j < countOfThisCard; j++) allCards.AddRange(cardsToClone);
        }

        return allCards.Count;
    }

    private static Card ParseCardFromLine(string inputLine)
    {
        var id = Convert.ToInt32(inputLine.Split(':')[0].Split(' ').Last());
        var numbers = inputLine.Split(':')[1].Split('|');

        var winningNumbersText = numbers[0];
        var myNumbersText = numbers[1];

        ICollection<int> GetParsedNumbersFromString(string numbersText)
            => numbersText
                .Split(' ')
                .Select(o => o.Trim())
                .Where(o => !string.IsNullOrWhiteSpace(o))
                .Select(o => Convert.ToInt32(o))
                .ToHashSet();

        return new Card(
            id,
            GetParsedNumbersFromString(winningNumbersText),
            GetParsedNumbersFromString(myNumbersText));
    }

    private record Card(int Id, ICollection<int> WinningNumbers, ICollection<int> MyNumbers)
    {
        public int MatchingNumbersCount { get => WinningNumbers.Count(MyNumbers.Contains); }

        public int Score
        {
            get => MatchingNumbersCount == 0
                ? 0
                : Convert.ToInt32(Math.Pow(2, MatchingNumbersCount - 1));
        }
    }
}
