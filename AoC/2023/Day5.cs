using System.Text.RegularExpressions;

namespace AoC._2023;

public static class Day5
{
    public static void Solve2()
    {
        var input = Extensions.ConsoleReadLinesBatchesSeparatedByEmptyLine();

        var seedsRegex = new Regex("(seeds: +)(?:(?<seed_range>(\\d+)( +)(\\d+)( *))+)+", RegexOptions.Compiled);

        var seeds = seedsRegex.Matches(input[0][0]).Single().Groups["seed_range"].Captures.ToArray()
            .Select(x => x.Value.Halve(" "))
            .SelectMany(x =>
            {
                var start = long.Parse(x.Item1);
                var range = long.Parse(x.Item2);
                return Range(start, range);
            })
            .Select(Convert.ToUInt64);

        var mapsOrdered = input
            .Skip(1)
            .Select(linesMap => new Map(linesMap.Skip(1)))
            .ToList();

        var lowestLocation = FindLowestLocation(seeds, mapsOrdered);

        Console.WriteLine(lowestLocation);
    }

    public static void Solve1()
    {
        var input = Extensions.ConsoleReadLinesBatchesSeparatedByEmptyLine();

        var seedsRegex = new Regex("\\d+", RegexOptions.Compiled);
        var seeds = seedsRegex.Matches(input[0][0])
            .Select(x => ulong.Parse(x.Value))
            .ToArray();

        var mapsOrdered = input
            .Skip(1)
            .Select(linesMap => new Map(linesMap.Skip(1)))
            .ToList();

        var lowestLocation = FindLowestLocation(seeds, mapsOrdered);

        Console.WriteLine(lowestLocation);
    }

    private static ulong FindLowestLocation(IEnumerable<ulong> seeds, List<Map> mapsOrdered)
    {
        var lowestLocation = ulong.MaxValue;
        foreach (var seed in seeds)
        {
            var position = seed;
            foreach (var map in mapsOrdered)
            {
                position = map.FindDst(position);
            }

            lowestLocation = Math.Min(lowestLocation, position);
        }

        return lowestLocation;
    }

    private static IEnumerable<long> Range(long start, long range)
    {
        for (var i = start; i <= start + range; i++)
        {
            yield return i;
        }
    }

    private class Map
    {
        private readonly Route[] routes;

        public Map(IEnumerable<string> lines)
        {
            routes = lines.Select(x => new Route(x)).ToArray();
        }

        public ulong FindDst(ulong src)
        {
            var mapped = routes.FirstOrDefault(x => x.FindDst(src) != null);
            if (mapped == null)
                return src;
            return (ulong)mapped.FindDst(src)!;
        }
    }

    private class Route
    {
        private static readonly Regex Regex = new("(?<dst_start>\\d+) +(?<src_start>\\d+) +(?<range>\\d+)",
            RegexOptions.Compiled);

        private readonly ulong dstStart;
        private readonly ulong range;
        private readonly ulong srcStart;

        public Route(string line)
        {
            var parsed = Regex.Matches(line).First();
            dstStart = ulong.Parse(parsed.Groups["dst_start"].Captures.First().Value);
            srcStart = ulong.Parse(parsed.Groups["src_start"].Captures.First().Value);
            range = ulong.Parse(parsed.Groups["range"].Captures.First().Value);
        }

        public ulong? FindDst(ulong src)
        {
            if (src >= srcStart && src <= srcStart + range)
                return dstStart + (src - srcStart);
            return null;
        }
    }
}