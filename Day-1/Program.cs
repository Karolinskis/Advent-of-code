using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "input.in";

            string pattern = @"\d|one|two|three|four|five|six|seven|eight|nine";

            int result = 0;
            using (var reader = new StreamReader(inputFile))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    var first = Regex.Match(line, pattern);
                    var last = Regex.Match(line, pattern, RegexOptions.RightToLeft);
                    result += parse(first.Value) * 10 + parse(last.Value); 
                }
            }

            Console.WriteLine(result);
        }

        private static int parse(string match) {
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
}
