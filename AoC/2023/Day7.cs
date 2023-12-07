using System.Text.RegularExpressions;

namespace AoC._2023;

public static class Day7
{
    public static void Solve2()
    {
        var input = Extensions.ConsoleReadLinesUntilEmptyLine();
        var regex = new Regex("(?<hand>[A,K,Q,J,T,9,8,7,6,5,4,3,2]+) +(?<bid>\\d+)", RegexOptions.Compiled);

        var result = input
            .Select(x =>
            {
                var groups = regex.Match(x).Groups;
                return (Hand: groups["hand"].Value, Bid: long.Parse(groups["bid"].Value));
            })
            .OrderBy(x => GetMaximumPossibleHandType(x.Hand))
            .ThenBy(x => GetCardValue(x.Hand[0]))
            .ThenBy(x => GetCardValue(x.Hand[1]))
            .ThenBy(x => GetCardValue(x.Hand[2]))
            .ThenBy(x => GetCardValue(x.Hand[3]))
            .ThenBy(x => GetCardValue(x.Hand[4]))
            .ToArray()
            .Select((x, i) => x.Bid * (i + 1))
            .Aggregate((x, y) => x + y);

        Console.WriteLine(result);

        HandType GetMaximumPossibleHandType(string hand)
        {
            var arr = hand.ToCharArray();
            var alphabet = arr.Distinct().Where(x => x != 'J').ToArray();

            var max = GetHandType(hand);
            foreach (var replacement in alphabet)
            {
                var newArr = new char[arr.Length];
                arr.CopyTo((Span<char>)newArr);
                for (var pos = 0; pos < arr.Length; pos++)
                {
                    if (arr[pos] == 'J')
                        newArr[pos] = replacement;
                }

                var newStr = new string(newArr);
                max = (HandType)Math.Max((int)max, (int)GetHandType(newStr));
            }

            return max;
        }

        int GetCardValue(char card)
        {
            return card switch
            {
                'A' => 1000,
                'K' => 900,
                'Q' => 800,
                'T' => 600,
                '9' => 9,
                '8' => 8,
                '7' => 7,
                '6' => 6,
                '5' => 5,
                '4' => 4,
                '3' => 3,
                '2' => 2,
                'J' => 1,
                _ => throw new ArgumentOutOfRangeException(card.ToString())
            };
        }
    }

    public static void Solve1()
    {
        var input = Extensions.ConsoleReadLinesUntilEmptyLine();
        var regex = new Regex("(?<hand>[A,K,Q,J,T,9,8,7,6,5,4,3,2]+) +(?<bid>\\d+)", RegexOptions.Compiled);

        var result = input
            .Select(x =>
            {
                var groups = regex.Match(x).Groups;
                return (Hand: groups["hand"].Value, Bid: long.Parse(groups["bid"].Value));
            })
            .OrderBy(x => GetHandType(x.Hand))
            .ThenBy(x => GetCardValue(x.Hand[0]))
            .ThenBy(x => GetCardValue(x.Hand[1]))
            .ThenBy(x => GetCardValue(x.Hand[2]))
            .ThenBy(x => GetCardValue(x.Hand[3]))
            .ThenBy(x => GetCardValue(x.Hand[4]))
            .ToArray()
            .Select((x, i) => x.Bid * (i + 1))
            .Aggregate((x, y) => x + y);

        Console.WriteLine(result);

        int GetCardValue(char card)
        {
            return card switch
            {
                'A' => 1000,
                'K' => 900,
                'Q' => 800,
                'J' => 700,
                'T' => 600,
                '9' => 9,
                '8' => 8,
                '7' => 7,
                '6' => 6,
                '5' => 5,
                '4' => 4,
                '3' => 3,
                '2' => 2,
                _ => throw new ArgumentOutOfRangeException(card.ToString())
            };
        }
    }


    private static HandType GetHandType(string hand)
    {
        var arr = hand.ToCharArray();
        if (arr.All(x => x.Equals(hand[0])))
            return HandType.FiveOfAKind;

        arr = arr.GroupBy(x => x)
            .OrderByDescending(x => x.Count())
            .SelectMany(x => x)
            .ToArray();

        if (arr.Take(4).All(x => x == arr[0]))
            return HandType.FourOfAKind;

        if (arr.Take(3).All(x => x == arr[0]) && arr.Skip(3).Take(2).All(x => x == arr[4]))
            return HandType.FullHouse;

        if (arr.Take(3).All(x => x == arr[0]))
            return HandType.ThreeOfAKind;

        if (arr.Take(2).All(x => x == arr[0]) && arr.Skip(2).Take(2).All(x => x == arr[3]))
            return HandType.TwoPair;

        if (arr.Take(2).All(x => x == arr[0]))
            return HandType.OnePair;

        return HandType.HighCard;
    }

    private enum HandType
    {
        FiveOfAKind = 1000,
        FourOfAKind = 900,
        FullHouse = 800,
        ThreeOfAKind = 700,
        TwoPair = 600,
        OnePair = 500,
        HighCard = 400
    }
}