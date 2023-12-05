using System.Text.RegularExpressions;

namespace AoC;

public static class Day3
{
    public static void Solve2()
    {
        var input = Extensions.ConsoleReadLinesUntilEmptyLine().ToArray();
        var width = input[0].Length;
        var height = input.Length;

        var map = GetAdjacentCellsMap(input).ToArray();
        var asterisksToNumbers = GetAsterisksToAdjacentNumbersMap(map);

        var result = asterisksToNumbers.Values
            .Where(value => value.Count > 1)
            .Select(c => c.Aggregate((c1, c2) => c1 * c2))
            .Aggregate((c1, c2) => c1 + c2);


        Console.WriteLine(result);

        IEnumerable<List<(int X, int Y)>[]> GetAdjacentCellsMap(string[] lines)
        {
            var cellsMap = new List<(int X, int Y)>[height][];
            var onlyAsterisksRegex = new Regex("\\*", RegexOptions.Compiled);

            for (var i = 0; i < height; i++)
            {
                cellsMap[i] = new List<(int X, int Y)>[width];
                for (var j = 0; j < width; j++)
                {
                    cellsMap[i][j] = new List<(int X, int Y)>();
                }
            }

            for (var y = 0; y < height; y++)
            {
                var matches = onlyAsterisksRegex.Matches(lines[y]).ToArray();
                foreach (var match in matches)
                {
                    var matchX = match.Index;
                    var matchY = y;
                    var digitAdjacentCells = GetAdjacentCells(y, matchX, width, height)
                        .Where(cell => char.IsDigit(lines[cell.Y][cell.X]))
                        .ToArray();
                    foreach (var cell in digitAdjacentCells)
                    {
                        cellsMap[cell.Y][cell.X].Add((matchY, matchX));
                    }
                }
            }

            return cellsMap;
        }

        Dictionary<(int X, int Y), List<ulong>> GetAsterisksToAdjacentNumbersMap(
            List<(int X, int Y)>[][] adjacentCellsMap)
        {
            var regex = new Regex("\\d{1,}", RegexOptions.Compiled);
            var dict = new Dictionary<(int X, int Y), List<ulong>>();
            for (var y = 0; y < height; y++)
            {
                var matches = regex.Matches(input[y]).ToArray();
                foreach (var match in matches)
                {
                    var matchY = y;
                    var number = ulong.Parse(match.Value);
                    var adjacentAsterisks = Enumerable.Range(match.Index, match.Length)
                        .SelectMany(x => adjacentCellsMap[matchY][x])
                        .Distinct();

                    foreach (var adjacentAsterisk in adjacentAsterisks)
                    {
                        if (dict.ContainsKey(adjacentAsterisk))
                            dict[adjacentAsterisk].Add(number);
                        else
                        {
                            dict[adjacentAsterisk] = new List<ulong> { number };
                        }
                    }
                }
            }

            return dict;
        }
    }

    public static void Solve1()
    {
        var input = Extensions.ConsoleReadLinesUntilEmptyLine().ToArray();
        var width = input[0].Length;
        var height = input.Length;
        var adjacentCells = GetAdjacentCellsMap();

        var regex = new Regex("\\d{1,}", RegexOptions.Compiled);
        var result = 0;
        for (var y = 0; y < height; y++)
        {
            var line = input[y];
            var matches = regex.Matches(line).ToArray();
            foreach (var match in matches)
            {
                for (var x = match.Index; x < match.Index + match.Value.Length; x++)
                {
                    if (adjacentCells[y][x])
                    {
                        result += int.Parse(match.Value);
                        break;
                    }
                }
            }
        }

        Console.WriteLine(result);

        bool[][] GetAdjacentCellsMap()
        {
            var map = new bool[height][];
            var onlySymbolsRegex = new Regex("[^.|\\d|\n]", RegexOptions.Compiled);

            for (var i = 0; i < height; i++)
            {
                map[i] = new bool[width];
            }

            for (var y = 0; y < height; y++)
            {
                var line = input[y];
                var symbols = onlySymbolsRegex.Matches(line).ToArray();
                foreach (var symbol in symbols)
                {
                    MarkAdjacentCells(y, symbol.Index, map);
                }
            }

            return map;
        }

        void MarkAdjacentCells(int y, int x, bool[][] map)
        {
            foreach (var cell in GetAdjacentCells(y, x, width, height))
            {
                map[cell.Y][cell.X] = true;
            }
        }
    }

    private static IEnumerable<(int X, int Y)> GetAdjacentCells(int y, int x, int width, int height)
    {
        var cells = new List<(int X, int Y)>();
        for (var dx = -1; dx <= 1; dx++)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                var newX = x + dx;
                var newY = y + dy;
                if (newX == x && newY == y)
                    continue;
                if (!IsWithinBoundaries(newX, newY, width, height))
                    continue;
                cells.Add((newX, newY));
            }
        }

        return cells;
    }

    private static bool IsWithinBoundaries(int x, int y, int width, int height)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}