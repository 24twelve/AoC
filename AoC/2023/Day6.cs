using System.Text.RegularExpressions;

namespace AoC._2023;

public static class Day6
{
    public static void Solve2()
    {
        var input = Extensions.ConsoleReadLinesUntilEmptyLine();

        var regex = new Regex("\\d+", RegexOptions.Compiled);

        var raceTimeLimits = long.Parse(regex.Matches(input[0]).Select(x => x.Value).Aggregate((x, y) => x + y));
        var recordDistances = long.Parse(regex.Matches(input[1]).Select(x => x.Value).Aggregate((x, y) => x + y));

        Solve(new[] { raceTimeLimits }, new[] { recordDistances });
    }

    public static void Solve1()
    {
        var input = Extensions.ConsoleReadLinesUntilEmptyLine();

        var regex = new Regex("\\d+", RegexOptions.Compiled);

        var raceTimeLimits = regex.Matches(input[0]).Select(x => long.Parse(x.Value)).ToArray();
        var recordDistances = regex.Matches(input[1]).Select(x => long.Parse(x.Value)).ToArray();

        Solve(raceTimeLimits, recordDistances);
    }

    private static void Solve(long[] raceTimeLimits, long[] recordDistances)
    {
        var result = 1;
        for (var i = 0; i < raceTimeLimits.Length; i++)
        {
            var raceTimeLimit = raceTimeLimits[i];
            var recordDistance = recordDistances[i];
            var winningHoldingTimesRange =
                FindQuadraticEquationRootsNonIncludingInterval(1, -raceTimeLimit, recordDistance)
                    .Where(x => x >= 0)
                    .ToArray();
            if (winningHoldingTimesRange.Length < 2)
                continue;

            var waysToWin = Math.Max(winningHoldingTimesRange[1] - winningHoldingTimesRange[0] + 1, 1);
            result *= waysToWin;
        }

        Console.WriteLine(result);

        IEnumerable<int> FindQuadraticEquationRootsNonIncludingInterval(double a, double b, double c)
        {
            var discriminant = Math.Sqrt(b * b - 4 * a * c);
            var first = (-1 * b - discriminant) / (2 * a);
            var second = (-1 * b + discriminant) / (2 * a);

            var left = first;
            var right = second;

            if (first > second)
            {
                left = second;
                right = first;
            }

            var leftInt = (int)Math.Ceiling(left);
            var rightInt = (int)Math.Floor(right);

            if (Math.Abs(Math.Ceiling(left) - left) < double.Epsilon)
            {
                leftInt++;
            }

            if (Math.Abs(Math.Floor(right) - right) < double.Epsilon)
            {
                rightInt--;
            }

            yield return leftInt;
            if (rightInt != leftInt)
                yield return rightInt;
        }
    }
}