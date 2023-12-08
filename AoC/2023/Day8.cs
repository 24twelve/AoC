using System.Text.RegularExpressions;

namespace AoC._2023;

public class Day8
{
    public static void Solve2()
    {
        var input = Extensions.ConsoleReadLinesBatchesSeparatedByEmptyLine();
        var instructions = input[0][0].Select(x => x == 'L' ? 0 : 1).ToArray();

        var regex = new Regex("[A-Z0-9]+");
        var graph = new Dictionary<string, string[]>();
        var startingNodes = new List<string>();
        foreach (var nodeLine in input[1])
        {
            var matches = regex.Matches(nodeLine).Select(x => x.Value).ToArray();
            var node = matches[0];
            graph[node] = new[] { matches[1], matches[2] };
            if (node.EndsWith('A'))
                startingNodes.Add(node);
        }

        var result = startingNodes
            .Select(x => Traverse(x, n => n.EndsWith('Z'), graph, instructions))
            .Aggregate(FindLeastCommonMultiple);

        Console.WriteLine(result);
    }

    public static void Solve1()
    {
        var input = Extensions.ConsoleReadLinesBatchesSeparatedByEmptyLine();
        var instructions = input[0][0].Select(x => x == 'L' ? 0 : 1).ToArray();

        var regex = new Regex("[A-Z0-9]+");
        var graph = new Dictionary<string, string[]>();
        foreach (var nodeLine in input[1])
        {
            var matches = regex.Matches(nodeLine).Select(x => x.Value).ToArray();
            graph[matches[0]] = new[] { matches[1], matches[2] };
        }

        var stepCount = Traverse("AAA", x => x == "ZZZ", graph, instructions);
        Console.WriteLine(stepCount);
    }

    private static long Traverse(string startPos, Func<string, bool> finishCondition,
        Dictionary<string, string[]> graph, int[] instructions)
    {
        var pointer = 0;
        var steps = 0;
        var currentPos = startPos;
        while (!finishCondition(currentPos))
        {
            currentPos = graph[currentPos][instructions[pointer]];
            steps++;
            pointer++;
            if (pointer >= instructions.Length)
                pointer = 0;
        }

        return steps;
    }

    private static long FindLeastCommonMultiple(long a, long b)
    {
        return a / FindGreatestCommonFactor(a, b) * b;
    }

    private static long FindGreatestCommonFactor(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }
}