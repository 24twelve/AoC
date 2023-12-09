using System.Text.RegularExpressions;

namespace AoC._2023;

public class Day9
{
    public static void Solve2()
    {
        var input = Extensions.ConsoleReadLinesUntilEmptyLine();
        var regex = new Regex("-?\\d+", RegexOptions.Compiled);
        var result = input
            .Select(x => regex.Matches(x)
                .Select(m => long.Parse(m.Value))
                .ToList())
            .Select(DeriveSequences)
            .Select(ProjectFirstValues)
            .Select(x => x.First().First())
            .Sum();

        Console.WriteLine(result);
    }
    
    public static void Solve1()
    {
        var input = Extensions.ConsoleReadLinesUntilEmptyLine();
        var regex = new Regex("-?\\d+", RegexOptions.Compiled);
        var result = input
            .Select(x => regex.Matches(x)
                .Select(m => long.Parse(m.Value))
                .ToList())
            .Select(DeriveSequences)
            .Select(ProjectNextValues)
            .Select(x => x.First().Last())
            .Sum();

        Console.WriteLine(result);
    }
    
    private static List<long>[] ProjectFirstValues(List<long>[] sequences)
    {
        var prev = sequences.Last();
        for (var i = sequences.Length - 2; i >= 0; i--)
        {
            var current = sequences[i];
            current.Insert(0,ExtrapolateFirst(current, prev));
            prev = current;
        }

        return sequences;
    }

    private static List<long>[] ProjectNextValues(List<long>[] sequences)
    {
        var prev = sequences.Last();
        for (var i = sequences.Length - 2; i >= 0; i--)
        {
            var current = sequences[i];
            current.Add(ExtrapolateNext(current, prev));
            prev = current;
        }

        return sequences;
    }
    
    private static long ExtrapolateFirst(List<long> current, List<long> prev)
    {
        var prevFirst = prev.First();
        var currentFirst = current.First();
        return currentFirst - prevFirst;
    }

    private static long ExtrapolateNext(List<long> current, List<long> prev)
    {
        var prevLast = prev.Last();
        var currentLast = current.Last();
        return currentLast + prevLast;
    }

    private static List<long>[] DeriveSequences(List<long> startSequence)
    {
        var result = new List<List<long>>() { startSequence };
        var prev = startSequence;
        while (true)
        {
            var allZeroes = TryDeriveSequence(prev, out var next);
            result.Add(next);
            if (allZeroes)
                break;
            prev = next;
        }

        return result.ToArray();
    }

    private static bool TryDeriveSequence(List<long> input, out List<long> derivative)
    {
        var result = new List<long>();
        var prev = input[0];
        var allZeroes = true;
        for (var i = 1; i < input.Count; i++)
        {
            var curr = input[i];
            var diff = curr - prev;
            if (diff != 0)
                allZeroes = false;
            result.Add(diff);
            prev = curr;
        }

        derivative = result;
        return allZeroes;
    }
}