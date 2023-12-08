namespace AOC2023.Net;

public class Day6
{
    public static long ExecutePart1()
    {
        List<(long Time, long Distance)> racesExample =
        [
            (7, 9),
            (15, 40),
            (30, 200)
        ];

        List<(long Time, long Distance)> racesFull =
        [
            (40, 215),
            (92, 1064),
            (97, 1505),
            (90, 1100)
        ];

        long result = 1;
        foreach (var race in racesFull)
        {
            result *= GetNumberOfWaysToWin(race);
        }

        return result;
    }

    public static long ExecutePart2()
    {
        return GetNumberOfWaysToWin((40929790, 215106415051100));
    }

    private static long GetNumberOfWaysToWin((long Time, long Distance) race)
    {
        for (int timeWaited = 0; timeWaited < race.Time; timeWaited++)
        {
            // Time waited is time gained so distance covered is time raced * time waited
            if ((race.Time - timeWaited) * timeWaited > race.Distance)
            {
                return race.Time + 1 - 2 * timeWaited;
            }
        }

        throw new Exception($"Unwinnable race: Time={race.Time}, Distance={race.Distance}");
    }
}
