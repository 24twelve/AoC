namespace AoC;

public static class Extensions
{
    public static string[][] ConsoleReadLinesBatchesSeparatedByEmptyLine()
    {
        var result = new List<string[]>();
        while (true)
        {
            var lines = ConsoleReadLinesUntilEmptyLine();
            if (lines.Any())
                result.Add(lines);
            else
                break;
        }

        return result.ToArray();
    }

    public static string[] ConsoleReadLinesUntilEmptyLine()
    {
        var lines = new List<string>();
        var line = Console.ReadLine();
        while (true)
        {
            if (string.IsNullOrEmpty(line))
                break;
            lines.Add(line);
            line = Console.ReadLine();
        }

        return lines.ToArray();
    }

    public static (string, string) Halve(this string str, string separator)
    {
        var split = str.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (split.Length != 2)
            throw new ArgumentException($"Cannot halve string '{str}' by '{separator}'");
        return (split[0], split[1]);
    }
}