using System.Text.RegularExpressions;

namespace AoC2023;

public static class Day1
{
    public static void Solve2()
    {
        var regex = new Regex("(?=(one|two|three|four|five|six|seven|eight|nine|1|2|3|4|5|6|7|8|9))",
            RegexOptions.Compiled);
        var map = new Dictionary<string, ulong>
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
            ["four"] = 4,
            ["five"] = 5,
            ["six"] = 6,
            ["seven"] = 7,
            ["eight"] = 8,
            ["nine"] = 9,
        };

        ulong result = 0;
        var lines = Extensions.ConsoleReadAllLines();
        foreach (var line in lines)
        {
            var matches = regex.Matches(line);
            var first = matches.First().Groups.Values.Last().Value;
            var last = matches.Last().Groups.Values.Last().Value;

            var res = new[] { last, first }.ToArray();

            for (var i = 1; i >= 0; i--)
            {
                result += (ulong)Math.Pow(10, i) * Parse(res[i]);
            }

            ulong Parse(string str)
            {
                return str.Length == 1 ? ulong.Parse(str) : map[str];
            }
        }

        Console.WriteLine(result);
    }
}