using Common;
using System.Data;
using System.Text.RegularExpressions;

namespace AoC_2024;

public class Day07 : Base2024Day
{
    private readonly string _input;

    public Day07()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var lines = _input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        long totalCalibrationResult = 0;

        foreach (var line in lines)
        {
            var parts = line.Split(":");
            long testValue = long.Parse(parts[0].Trim());
            long[] numbers = parts[1].Trim().Split(' ').Select(long.Parse).ToArray();

            if (CanBeTrue(testValue, numbers))
            {
                totalCalibrationResult += testValue;
            }
        }

        return new ValueTask<string>(totalCalibrationResult.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var lines = _input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        long totalCalibrationResult = 0;

        foreach (var line in lines)
        {
            var parts = line.Split(":");
            long testValue = long.Parse(parts[0].Trim());
            long[] numbers = parts[1].Trim().Split(' ').Select(long.Parse).ToArray();

            if (CanBeTrue(testValue, numbers, true))
            {
                totalCalibrationResult += testValue;
            }
        }

        return new ValueTask<string>(totalCalibrationResult.ToString());
    }

    private bool CanBeTrue(long testValue, long[] numbers, bool useConcatenation = false)
    {
        return Evaluate(numbers, 0, numbers[0], testValue, useConcatenation);
    }

    private bool Evaluate(long[] numbers, int index, long currentValue, long testValue, bool useConcatenation = false)
    {
        if (index == numbers.Length - 1)
        {
            return currentValue == testValue;
        }

        int nextIndex = index + 1;
        long nextNumber = numbers[nextIndex];

        // Try addition
        if (Evaluate(numbers, nextIndex, currentValue + nextNumber, testValue, useConcatenation))
        {
            return true;
        }

        // Try multiplication
        if (Evaluate(numbers, nextIndex, currentValue * nextNumber, testValue, useConcatenation))
        {
            return true;
        }

        // Try concatenation
        if (useConcatenation)
        {
            if (Evaluate(numbers, nextIndex, long.Parse(currentValue.ToString() + nextNumber), testValue, useConcatenation))
            {
                return true;
            }
        }

        return false;
    }
}
