using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() 
    {
        int result = 0;

        using (var reader = new StringReader(_input))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                int left = 0;
                int right = 0;
                // Left
                for(int i = 0; i < line.Length; i++) {
                    if (Char.IsDigit(line[i])) {
                        left = line[i] - '0';
                        break;
                    }
                }

                // Right
                for(int i = line.Length - 1; i >= 0; i--) {
                    if (Char.IsDigit(line[i])) {
                        right = line[i] - '0';
                        break;
                    }
                }

                result += left * 10 + right;
            }
        }

        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2() 
    {
        int result = 0;
        string pattern = @"\d|one|two|three|four|five|six|seven|eight|nine";

        using (var reader = new StringReader(_input))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var first = Regex.Match(line, pattern);
                var last = Regex.Match(line, pattern, RegexOptions.RightToLeft);
                result += Parse(first.Value) * 10 + Parse(last.Value);
            }
        }

        return new ValueTask<string>($"{result}");
    } 

    private static int Parse(string match)
    {
        return match switch
        {
            "one" => 1,
            "two" => 2,
            "three" => 3,
            "four" => 4,
            "five" => 5,
            "six" => 6,
            "seven" => 7,
            "eight" => 8,
            "nine" => 9,
            _ => int.Parse(match)
        };        
    }
}
