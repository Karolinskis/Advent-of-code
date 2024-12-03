using Common;
using System.Text.RegularExpressions;

namespace AoC_2024;

public class Day02 : Base2024Day
{
    private readonly string _input;

    public Day02()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        int safeReports = 0;

        foreach (string report in _input.Split("\n", StringSplitOptions.RemoveEmptyEntries))
        {
            List<int> reportItems = report.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            if (IsSafe(reportItems))
            {
                safeReports++;
            }
        }

        return new ValueTask<string>($"{safeReports}");
    }

    public override ValueTask<string> Solve_2()
    {
        int safeReports = 0;

        foreach (string report in _input.Split("\n", StringSplitOptions.RemoveEmptyEntries))
        {
            List<int> reportItems = report.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            if (IsSafe(reportItems))
            {
                safeReports++;
                continue;
            }
            else
            {

                bool canBeMadeSafe = false;

                // Check if removing one level makes the report safe
                for (int i = 0; i < reportItems.Count; i++)
                {
                    var modifiedReport = new List<int>(reportItems);
                    modifiedReport.RemoveAt(i);

                    if (IsSafe(modifiedReport))
                    {
                        canBeMadeSafe = true;
                        break;
                    }
                }

                if (canBeMadeSafe)
                {
                    safeReports++;
                }
            }
        }

        return new ValueTask<string>($"{safeReports}");
    }

    private static bool IsSafe(List<int> reportItems)
    {
        bool isAscending = true;
        bool isDecreasing = true;

        for (int i = 1; i < reportItems.Count; i++)
        {
            int difference = Math.Abs(reportItems[i] - reportItems[i - 1]);
            if (difference < 1 || difference > 3)
            {
                isAscending = false;
                isDecreasing = false;
                break;
            }

            if (reportItems[i] > reportItems[i - 1])
            {
                isDecreasing = false;
            }
            else if (reportItems[i] < reportItems[i - 1])
            {
                isAscending = false;
            }
        }

        return isAscending || isDecreasing;
    }
}
