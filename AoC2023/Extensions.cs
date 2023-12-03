namespace AoC2023;

public static class Extensions
{
    public static string[] ConsoleReadAllLines()
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
}