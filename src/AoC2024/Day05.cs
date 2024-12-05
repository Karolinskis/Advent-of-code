using Common;
using System.Text.RegularExpressions;

namespace AoC_2024;

public class Day05 : Base2024Day
{
    private readonly string _input;

    public Day05()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var (rules, updates) = ParseInput(_input);
        var validUpdates = updates.Where(update => IsValidUpdate(update, rules)).ToList();
        var middlePageNumbers = validUpdates.Select(GetMiddlePageNumber).ToList();
        var result = middlePageNumbers.Sum();

        return new ValueTask<string>(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var (rules, updates) = ParseInput(_input);
        var invalidUpdates = updates.Where(update => !IsValidUpdate(update, rules)).ToList();
        var correctedUpdates = invalidUpdates.Select(update => CorrectOrder(update, rules)).ToList();
        var middlePageNumbers = correctedUpdates.Select(GetMiddlePageNumber).ToList();
        var result = middlePageNumbers.Sum();

        return new ValueTask<string>(result.ToString());
    }

    private (List<(int, int)> rules, List<List<int>> updates) ParseInput(string input)
    {
        var sections = input.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
        if (sections.Length != 2)
        {
            throw new FormatException("Input does not contain exactly two sections separated by a blank line.");
        }

        var rules = sections[0].Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(line =>
            {
                var parts = line.Split('|');
                if (parts.Length != 2)
                {
                    throw new FormatException($"Invalid rule format: {line}");
                }
                return (int.Parse(parts[0]), int.Parse(parts[1]));
            })
            .ToList();


        var updates = sections[1].Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(line =>
            {
                var parts = line.Split(',');
                if (parts.Length == 0)
                {
                    throw new FormatException($"Invalid update format: {line}");
                }
                return parts.Select(int.Parse).ToList();
            })
            .ToList();

        return (rules, updates);
    }

    private bool IsValidUpdate(List<int> update, List<(int, int)> rules)
    {
        var indexMap = update.Select((page, index) => (page, index)).ToDictionary(pair => pair.page, pair => pair.index);
        foreach (var (x, y) in rules)
        {
            if (indexMap.ContainsKey(x) && indexMap.ContainsKey(y) && indexMap[x] > indexMap[y])
            {
                return false;
            }
        }

        return true;
    }

    private int GetMiddlePageNumber(List<int> update)
    {
        return update[update.Count / 2];
    }

    private List<int> CorrectOrder(List<int> update, List<(int, int)> rules)
    {
        var graph = new Dictionary<int, List<int>>();
        var inDegree = new Dictionary<int, int>();

        foreach (var page in update)
        {
            graph[page] = new List<int>();
            inDegree[page] = 0;
        }

        foreach (var (x, y) in rules)
        {
            if (graph.ContainsKey(x) && graph.ContainsKey(y))
            {
                graph[x].Add(y);
                inDegree[y]++;
            }
        }

        var queue = new Queue<int>(update.Where(page => inDegree[page] == 0));
        var sorted = new List<int>();

        while (queue.Count > 0)
        {
            var page = queue.Dequeue();
            sorted.Add(page);

            foreach (var neighbor in graph[page])
            {
                inDegree[neighbor]--;
                if (inDegree[neighbor] == 0)
                {
                    queue.Enqueue(neighbor);
                }
            }
        }

        return sorted;
    }
}
