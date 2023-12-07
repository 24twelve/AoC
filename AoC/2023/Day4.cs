using System.Text.RegularExpressions;

namespace AoC._2023;

public static class Day4
{
    public static void Solve1()
    {
        var input = Extensions.ConsoleReadLinesUntilEmptyLine();
        var result = input
            .Select(card => ProcessCard(card).WinningNumberCount)
            .Where(cardResultPower => cardResultPower > 0)
            .Aggregate<int, ulong>(0, (current, cardResultPower) => current + (ulong)Math.Pow(2, cardResultPower));

        Console.WriteLine(result);
    }

    public static void Solve2()
    {
        var input = Extensions.ConsoleReadLinesUntilEmptyLine();
        var processed = input
            .Select(ProcessCard)
            .ToDictionary(key => key.CardNumber, value => value.WinningNumberCount);

        var result = 0;
        var queue = new Queue<int>(processed.Keys);
        while (queue.Any())
        {
            var cardNumber = queue.Dequeue();
            result++;
            foreach (var copyCardNumber in Enumerable.Range(cardNumber + 1, processed[cardNumber]))
            {
                queue.Enqueue(copyCardNumber);
            }
        }

        Console.WriteLine(result);
    }

    private static (int CardNumber, int WinningNumberCount) ProcessCard(string card)
    {
        var (cardNumber, playingNumbersArr, winningNumbers) = ParseCard(card);
        return (cardNumber, GetWinningNumbersCount(playingNumbersArr, winningNumbers));
    }

    private static int GetWinningNumbersCount(string[] playingNumbers, string[] winningNumbers)
    {
        var playingNumbersSet = playingNumbers.ToHashSet();
        return winningNumbers.Count(winningNumber => playingNumbersSet.Contains(winningNumber));
    }

    private static (int CardNumber, string[] PlayingNumbers, string[] WinningNumbers) ParseCard(string card)
    {
        var regex = new Regex(
            "(Card +(?<card_number>\\d+): +)(?:((?<playing_number>\\d+)( *))+) +\\| +(?:((?<winning_number>\\d+)( *))+)",
            RegexOptions.Compiled);
        var group = regex.Matches(card).First().Groups;
        var cardNumber = group["card_number"].Captures.First();
        var playingNumbers = group["playing_number"].Captures.Select(x => x.Value).ToArray();
        var winningNumbers = group["winning_number"].Captures.Select(x => x.Value).ToArray();
        return (int.Parse(cardNumber.Value), playingNumbers, winningNumbers);
    }
}