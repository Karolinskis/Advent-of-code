using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Day2
{
    class Program
    {
        const int RED_CUBES = 12;
        const int GREEN_CUBES = 13;
        const int BLUE_CUBES = 14;

        static void Main(string[] args)
        {
            string inputFile = "input.in";
            


            int gameIDs = 0;
            int powers = 0;

            using(var reader = new StreamReader(inputFile))
            {
                string? line;
                int gameID = 1;
                while ((line = reader.ReadLine()) != null)
                {
                    gameIDs += isPossible(line) ? gameID : 0;
                    powers += getMinimumPower(line);
                    gameID++;
                }
            }

            Console.WriteLine("Games: " + gameIDs);
            Console.WriteLine("Power: " + powers);

        }

        private static bool isPossible(string line)
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
}
