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
        var startingCards = File.ReadAllLines("../inputs/4.txt")
            .Select(ParseCardFromLine)
            .ToList();

        var countsOfCardsById = startingCards.ToDictionary(o => o.Id, o => 1);

        foreach (var card in startingCards)
        {
            var numberToGoUp = card.MatchingNumbersCount;
            var currentCountOfThisCard = countsOfCardsById[card.Id];

            foreach (var entry in countsOfCardsById.Where(o => o.Key > card.Id && o.Key <= card.Id + numberToGoUp))
            {
                countsOfCardsById[entry.Key] = entry.Value + currentCountOfThisCard;
            }
        }

        return countsOfCardsById.Sum(o => o.Value);
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
