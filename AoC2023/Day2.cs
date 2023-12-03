using System.Text.RegularExpressions;

namespace AoC2023;

public static class Day2
{
    public static void Solve1()
    {
        var lines = Extensions.ConsoleReadAllLines();

        var result = 0;
        for (var i = 0; i < lines.Length; i++)
        {
            var takes = Parse(lines[i]);
            if (AreAllPossible(takes))
            {
                var gameNumber = i + 1;
                result += gameNumber;
            }
        }

        Console.WriteLine(result);
    }

    public static void Solve2()
    {
        var lines = Extensions.ConsoleReadAllLines();

        var result = 0;

        foreach (var line in lines)
        {
            var maxRed = 1;
            var maxGreen = 1;
            var maxBlue = 1;

            var takes = Parse(line);
            foreach (var take in takes)
            {
                maxRed = Math.Max(maxRed, take.Red);
                maxGreen = Math.Max(maxGreen, take.Green);
                maxBlue = Math.Max(maxBlue, take.Blue);
            }

            var power = maxRed * maxGreen * maxBlue;
            if (power == 1)
                power = 0;
            result += power;
        }

        Console.WriteLine(result);
    }

    private static bool AreAllPossible(IEnumerable<(int Red, int Green, int Blue)> takes)
    {
        const int maxRed = 12;
        const int maxBlue = 14;
        const int maxGreen = 13;

        return takes.All(x => x is { Red: <= maxRed, Blue: <= maxBlue, Green: <= maxGreen });
    }

    private static IEnumerable<(int Red, int Green, int Blue)> Parse(string input)
    {
        input += ";";
        var regex1 = new Regex("(Game (?<game>\\d{1,}): )?(?<round>(.*?)(;))", RegexOptions.Compiled);
        var regex2 = new Regex("(?<count>\\d{1,})( )(?<color>.*?)(,|;)", RegexOptions.Compiled);

        var rounds = regex1.Matches(input).Select(x => x.Groups["round"]).ToArray();

        foreach (var round in rounds)
        {
            var takes = regex2.Matches(round.Value).ToArray();

            var red = 0;
            var green = 0;
            var blue = 0;

            foreach (var take in takes)
            {
                var value = take.Groups["count"].Value;
                var count = int.Parse(value);
                var color = take.Groups["color"].Value;
                switch (color)
                {
                    case "red":
                        red = count;
                        break;
                    case "blue":
                        blue = count;
                        break;
                    case "green":
                        green = count;
                        break;
                }
            }

            yield return (red, green, blue);
        }
    }
}