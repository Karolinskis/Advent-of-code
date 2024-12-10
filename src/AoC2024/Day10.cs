using Common;
using System.Data;
using System.Text.RegularExpressions;

namespace AoC_2024;

public class Day10 : Base2024Day
{
    private readonly string _input;

    public Day10()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var map = ParseMap(_input);
        var trailheads = FindTrailHeads(map);
        int totalScore = trailheads.Sum(th => CalculateScore(map, th));
        return new ValueTask<string>(totalScore.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var map = ParseMap(_input);
        var trailheads = FindTrailHeads(map);
        int totalScore = trailheads.Sum(th => CalculateRating(map, th));
        return new ValueTask<string>(totalScore.ToString());
    }

    private int[,] ParseMap(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        int rows = lines.Length;
        int cols = lines[0].Length;
        var map = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                map[i, j] = lines[i][j] - '0';
            }
        }

        return map;
    }

    private List<(int, int)> FindTrailHeads(int[,] map)
    {
        var trailheads = new List<(int, int)>();
        var rows = map.GetLength(0);
        var cols = map.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (map[i, j] == 0)
                {
                    trailheads.Add((i, j));
                }
            }
        }

        return trailheads;
    }

    private int CalculateScore(int[,] map, (int, int) trailhead)
    {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);
        var directions = new (int, int)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        var visited = new bool[rows, cols];
        var queue = new Queue<(int, int, int)>();
        queue.Enqueue((trailhead.Item1, trailhead.Item2, 0));
        visited[trailhead.Item1, trailhead.Item2] = true;
        int score = 0;

        while (queue.Count > 0)
        {
            var (x, y, height) = queue.Dequeue();

            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < rows && ny >= 0 && ny < cols && !visited[nx, ny] && map[nx, ny] == height + 1)
                {
                    if (map[nx, ny] == 9)
                    {
                        score++;
                    }
                    queue.Enqueue((nx, ny, map[nx, ny]));
                    visited[nx, ny] = true;
                }
            }
        }

        return score;
    }

    private int CalculateRating(int[,] map, (int, int) trailhead)
    {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);
        var directions = new (int, int)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        var visited = new bool[rows, cols];
        return DFS(map, trailhead.Item1, trailhead.Item2, 0, visited, directions);
    }

    private int DFS(int[,] map, int x, int y, int height, bool[,] visited, (int, int)[] directions)
    {
        if (map[x, y] != height) return 0;
        if (map[x, y] == 9) return 1;

        visited[x, y] = true;
        int count = 0;

        foreach (var (dx, dy) in directions)
        {
            int nx = x + dx;
            int ny = y + dy;

            if (nx >= 0 && nx < map.GetLength(0) && ny >= 0 && ny < map.GetLength(1) && !visited[nx, ny])
            {
                count += DFS(map, nx, ny, height + 1, visited, directions);
            }
        }

        visited[x, y] = false;
        return count;
    }
}
