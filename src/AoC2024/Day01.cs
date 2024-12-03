using Common;
using System.Text.RegularExpressions;

namespace AoC_2024;

public class Day01 : Base2024Day
{
    private readonly string _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<int> leftIDs = new();
        List<int> rightIDs = new();

        foreach (var line in _input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            leftIDs.Add(Parse(line, 0));
            rightIDs.Add(Parse(line, 1));
        }

        List<int> distances = new();

        while (leftIDs.Count > 0 && rightIDs.Count > 0)
        {
            int minLeft = leftIDs.Min();
            int minLeftIndex = leftIDs.IndexOf(minLeft);

            int minRight = rightIDs.Min();
            int minRightIndex = rightIDs.IndexOf(minRight);

            distances.Add(Math.Abs(minLeft - minRight));

            leftIDs.RemoveAt(minLeftIndex);
            rightIDs.RemoveAt(minRightIndex);
        }

        return new ValueTask<string>($"{distances.Sum()}");
    }

    public override ValueTask<string> Solve_2()
    {
        List<int> leftIDs = new();
        List<int> rightIDs = new();

        foreach (var line in _input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            leftIDs.Add(Parse(line, 0));
            rightIDs.Add(Parse(line, 1));
        }

        int similarityScore = 0;

        foreach (var left in leftIDs)
        {
            int appearCount = 0;

            foreach (var right in rightIDs)
            {
                if (left == right)
                {
                    appearCount++;
                }
            }

            similarityScore += left * appearCount;
        }

        return new ValueTask<string>($"{similarityScore}");
    }

    private static int Parse(string line, int index)
    {
        var items = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return int.Parse(items[index]);
    }
}
