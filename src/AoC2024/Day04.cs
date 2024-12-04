using Common;
using System.Text.RegularExpressions;

namespace AoC_2024;

public class Day04 : Base2024Day
{
    private readonly string _input;

    public Day04()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var grid = Parseinput(_input);
        int count = CountOccurrences(grid, "XMAS");
        return new ValueTask<string>(count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var grid = Parseinput(_input);
        int count = CountXMASOccurrences(grid);
        return new ValueTask<string>(count.ToString());
    }

    private char[][] Parseinput(string input)
    {
        var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var grid = new char[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            grid[i] = lines[i].ToCharArray();
        }

        return grid;
    }

    private int CountOccurrences(char[][] grid, string word)
    {
        int count = 0;
        int rows = grid.Length;
        int cols = grid[0].Length;
        int wordLength = word.Length;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (grid[row][col] == word[0])
                {
                    count += SearchWord(grid, word, row, col, 0, 1); // Horizontal right
                    count += SearchWord(grid, word, row, col, 0, -1); // Horizontal left
                    count += SearchWord(grid, word, row, col, 1, 0); // Vertical down
                    count += SearchWord(grid, word, row, col, -1, 0); // Vertical up
                    count += SearchWord(grid, word, row, col, 1, 1); // Diagonal down right
                    count += SearchWord(grid, word, row, col, 1, -1); // Diagonal down left
                    count += SearchWord(grid, word, row, col, -1, 1); // Diagonal up right
                    count += SearchWord(grid, word, row, col, -1, -1); // Diagonal up left
                }
            }
        }

        return count;
    }

    private int SearchWord(char[][] grid, string word, int row, int col, int rowDir, int colDir)
    {
        int wordLength = word.Length;
        for (int i = 0; i < wordLength; i++)
        {
            int newRow = row + i * rowDir;
            int newCol = col + i * colDir;
            if (newRow < 0 ||
                newRow >= grid.Length ||
                newCol < 0 ||
                newCol >= grid[0].Length ||
                grid[newRow][newCol] != word[i])
            {
                return 0;
            }
        }
        return 1;
    }

    private int CountXMASOccurrences(char[][] grid)
    {
        int count = 0;
        int rows = grid.Length;
        int cols = grid[0].Length;

        for (int row = 0; row < rows - 2; row++)
        {
            for (int col = 0; col < cols - 2; col++)
            {
                count += CheckXMAS(grid, row, col);
            }
        }

        return count;
    }

    private int CheckXMAS(char[][] grid, int row, int col)
    {
        // M . S
        // . A .
        // M . S
        if (grid[row][col] == 'M' && grid[row][col + 2] == 'S' &&
            grid[row + 1][col + 1] == 'A' &&
            grid[row + 2][col] == 'M' && grid[row + 2][col + 2] == 'S')
        {
            return 1;
        }

        // S . M
        // . A .
        // S . M
        if (grid[row][col] == 'S' && grid[row][col + 2] == 'M' &&
            grid[row + 1][col + 1] == 'A' &&
            grid[row + 2][col] == 'S' && grid[row + 2][col + 2] == 'M')
        {
            return 1;
        }

        // S . S
        // . A .
        // M . M
        if (grid[row][col] == 'S' && grid[row][col + 2] == 'S' &&
            grid[row + 1][col + 1] == 'A' &&
            grid[row + 2][col] == 'M' && grid[row + 2][col + 2] == 'M')
        {
            return 1;
        }

        // M . M
        // . A .
        // S . S
        if (grid[row][col] == 'M' && grid[row][col + 2] == 'M' &&
            grid[row + 1][col + 1] == 'A' &&
            grid[row + 2][col] == 'S' && grid[row + 2][col + 2] == 'S')
        {
            return 1;
        }

        return 0;
    }
}
