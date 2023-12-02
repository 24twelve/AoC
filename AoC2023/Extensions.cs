namespace AoC2023;

public static class Extensions
{
    public static string[] ConsoleReadAllLines()
    {
        var lines = new List<string>();
        var line = Console.ReadLine();
        while (!string.IsNullOrEmpty(line))
        {
            line += ";";
            lines.Add(line);
            line = Console.ReadLine();
        }

        return lines.ToArray();
    }
}