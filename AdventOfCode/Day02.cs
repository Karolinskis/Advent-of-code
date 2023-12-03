using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day02 : BaseDay
{
    private readonly string _input;
    private readonly int RED_CUBES = 12;
    private readonly int GREEN_CUBES = 13;
    private readonly int BLUE_CUBES = 14;


    public Day02()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() 
    {
        int gameID = 1;
        int gameIDs = 0;

        using (var reader = new StringReader(_input))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                gameIDs += isPossible(line) ? gameID : 0;
                gameID++;
            }
        }

        return new ValueTask<string>($"{gameIDs}");
    }

    public override ValueTask<string> Solve_2() 
    {
        int power = 0;

        using (var reader = new StringReader(_input))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                power += getMinimumPower(line);
            }
        }

        return new ValueTask<string>($"{power}");
    } 

    private bool isPossible(string line)
    {
        string[] split = line.Split(':', ';');

        for (int i = 1; i < split.Length; i++) // Ignore Game X:
        {
            int redCount = 0;
            int greenCount = 0;
            int blueCount = 0;

            string[] pull = split[i].Split(',', StringSplitOptions.TrimEntries);
            foreach (string cube in pull) 
            {
                if(cube.Contains("red"))
                {
                    string[] parts = cube.Split(' ');
                    redCount = int.Parse(parts[0]);
                    continue;
                }

                if(cube.Contains("green"))
                {
                    string[] parts = cube.Split(' ');
                    greenCount = int.Parse(parts[0]);
                    continue;
                }

                if(cube.Contains("blue"))
                {
                    string[] parts = cube.Split(' ');
                    blueCount = int.Parse(parts[0]);
                    continue;
                }
            }

            if( redCount > RED_CUBES ||
                greenCount > GREEN_CUBES ||
                blueCount > BLUE_CUBES)
            {
                return false;
            }
        }

        return true;
    }

    private static int getMinimumPower(string line)
    {
        string[] split = line.Split(':', ';');

        int minReds = int.MinValue;
        int minGreens = int.MinValue;
        int minBlues = int.MinValue;

        for (int i = 1; i < split.Length; i++) // Ignore Game X:
        {
            string[] pull = split[i].Split(',', StringSplitOptions.TrimEntries);
            foreach (string cube in pull) 
            {
                if(cube.Contains("red"))
                {
                    string[] parts = cube.Split(' ');
                    minReds = Math.Max(minReds, int.Parse(parts[0]));
                    continue;
                }

                if(cube.Contains("green"))
                {
                    string[] parts = cube.Split(' ');
                    minGreens = Math.Max(minGreens, int.Parse(parts[0]));
                    continue;
                }

                if(cube.Contains("blue"))
                {
                    string[] parts = cube.Split(' ');
                    minBlues = Math.Max(minBlues, int.Parse(parts[0]));
                    continue;
                }
            }
        }
        return minReds * minGreens * minBlues;
    }
}
