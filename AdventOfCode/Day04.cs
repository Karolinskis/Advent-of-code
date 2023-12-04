namespace AdventOfCode;

public class Day04 : BaseDay
{
    private readonly string[] _input;


    public Day04()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        int result = 0;

        foreach (string line in _input)
        {
            int lineResult = 0;

            string[] split = line.Split('|');
            string[] winningNumbersStr = split[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] numbersWeHaveStr = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            int[] winningNumbers = Array.ConvertAll(winningNumbersStr, int.Parse);
            int[] numbersWeHave = Array.ConvertAll(numbersWeHaveStr, int.Parse);

            for (int i = 0; i < winningNumbers.Length; i++)
            {
                for (int j = 0; j < numbersWeHave.Length; j++)
                {
                    if (winningNumbers[i] == numbersWeHave[j])
                    {
                        if(lineResult == 0) lineResult = 1;
                        else lineResult *= 2;
                    }
                }
            }

            result += lineResult;
        }

        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        Dictionary<int, int> pairs = new Dictionary<int, int>(); // <lineNum, ticketCount>
        for (int i = 0; i < _input.Length; i++)
            pairs.Add(i + 1, 1);

        for (int i = 0; i < _input.Length; i++)
        {
            string line = _input[i];

            string[] split = line.Split('|');
            string[] winningNumbersStr = split[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] numbersWeHaveStr = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            int[] winningNumbers = Array.ConvertAll(winningNumbersStr, int.Parse);
            int[] numbersWeHave = Array.ConvertAll(numbersWeHaveStr, int.Parse);

            for (int j = 0; j < winningNumbers.Intersect(numbersWeHave).Count(); j++)
            {
                pairs[i + 1 + j + 1] += pairs[i + 1];
            }
        }   

        int result = 0;
        foreach (var pair in pairs)
        {
            result += pair.Value;
        }

        return new ValueTask<string>($"{result}");
    }
}