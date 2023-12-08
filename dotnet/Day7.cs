namespace AOC2023.Net;

public class Day7
{
    public static int ExecutePart1()
    {
        var lines = File.ReadAllLines("../inputs/7.txt");
        var hands = lines.Select(o => ParseHand(o));

        var handsOrderedByRank = hands.OrderByDescending(h => h.Type)
            .ThenBy(h => h.Cards[0])
            .ThenBy(h => h.Cards[1])
            .ThenBy(h => h.Cards[2])
            .ThenBy(h => h.Cards[3])
            .ThenBy(h => h.Cards[4])
            .ToList();

        return handsOrderedByRank.Select((hand, index) =>
        {
            return (index + 1) * hand.Bid;
        }).Sum();
    }

    public static int ExecutePart2()
    {
        var lines = File.ReadAllLines("../inputs/7.txt");
        var hands = lines.Select(o => ParseHand(o, jokersSuck: true));

        var handsOrderedByRank = hands.OrderByDescending(h => h.Type)
            .ThenBy(h => h.Cards[0])
            .ThenBy(h => h.Cards[1])
            .ThenBy(h => h.Cards[2])
            .ThenBy(h => h.Cards[3])
            .ThenBy(h => h.Cards[4])
            .ToList();

        return handsOrderedByRank.Select((hand, index) =>
        {
            return (index + 1) * hand.Bid;
        }).Sum();
    }

    private static Hand ParseHand(string input, bool jokersSuck = false)
    {
        var split = input.Split(' ');

        var cards = split[0].Select(card =>
        {
            if (int.TryParse(card.ToString(), out var digit)) return digit;
            return card switch
            {
                'T' => 10,
                'J' => jokersSuck ? 11 : 0,
                'Q' => 12,
                'K' => 13,
                'A' => 14,
                _ => throw new ArgumentException(nameof(card))
            };
        }).ToList();

        var type = GetHandType(cards);

        return new Hand(cards, type, int.Parse(split[1]));
    }

    private static HandType GetHandType(ICollection<int> cards)
    {
        var cardCounts = cards
            .Where(card => card != 0) // Filter out zeros (Jokers) to apply later
            .GroupBy(o => o)
            .Select(o => o.Count())
            .OrderByDescending(o => o)
            .ToArray();

        // Have to handle 5 jokers!
        if (cardCounts.Length == 0) return HandType.FiveOfAKind;

        cardCounts[0] += cards.Count(c => c == 0);

        if (cardCounts[0] == 5) return HandType.FiveOfAKind;
        if (cardCounts[0] == 4 && cardCounts[1] == 1) return HandType.FourOfAKind;
        if (cardCounts[0] == 3)
        {
            if (cardCounts[1] == 2) return HandType.FullHouse;
            return HandType.ThreeOfAKind;
        }
        if (cardCounts[0] == 2)
        {
            if (cardCounts[1] == 2) return HandType.TwoPair;
            return HandType.OnePair;
        }

        return HandType.HighCard;
    }

    private record Hand(IList<int> Cards, HandType Type, int Bid);

    private enum HandType
    {
        FiveOfAKind,
        FourOfAKind,
        FullHouse,
        ThreeOfAKind,
        TwoPair,
        OnePair,
        HighCard
    }
}
