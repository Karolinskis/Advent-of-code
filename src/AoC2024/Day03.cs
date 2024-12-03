using Common;
using System.Text.RegularExpressions;

namespace AoC_2024;

public class Day03 : Base2024Day
{
    private readonly string _input;

    public Day03()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        string input = _input;

        int result = 0;
        var pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
        var regex = new Regex(pattern);

        while (input.Length > 0)
        {
            var match = regex.Match(input);
            if (!match.Success)
            {
                break;
            }

            var num1 = int.Parse(match.Groups[1].Value);
            var num2 = int.Parse(match.Groups[2].Value);

            result += num1 * num2;

            input = regex.Replace(input, "", 1);
        }

        return new ValueTask<string>(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        string input = _input;
        int result = 0;
        var pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
        var regex = new Regex(pattern);

        while (input.Length > 0)
        {
            if (input.Contains("don't()"))
            {
                int dontIndex = input.IndexOf("don't()");
                int doIndex = input.IndexOf("do()", dontIndex);

                if (doIndex != -1)
                {
                    input = input.Remove(dontIndex, doIndex - dontIndex + "do()".Length);
                }
                else
                {
                    input = input.Substring(0, dontIndex);
                }
            }

            var match = regex.Match(input);
            if (!match.Success)
            {
                break;
            }

            var num1 = int.Parse(match.Groups[1].Value);
            var num2 = int.Parse(match.Groups[2].Value);

            result += num1 * num2;

            input = regex.Replace(input, "", 1);
        }

        return new ValueTask<string>(result.ToString());
    }
}
