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
        throw new NotImplementedException();
    }

    private static Card ParseCardFromLine(string inputLine)
    {
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
            GetParsedNumbersFromString(winningNumbersText),
            GetParsedNumbersFromString(myNumbersText));
    }

    private record Card(ICollection<int> WinningNumbers, ICollection<int> MyNumbers)
    {
        public int MatchingNumbers { get => WinningNumbers.Count(MyNumbers.Contains); }

        public int Score
        {
            get => MatchingNumbers == 0
                ? 0
                : Convert.ToInt32(Math.Pow(2, MatchingNumbers - 1));
        }
    }
}
