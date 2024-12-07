using Common;
using System.Data;
using System.Text.RegularExpressions;

namespace AoC_2024;

public class Day06 : Base2024Day
{
    private record Guard((int, int) Position, char Direction)
    {
        public char Direction { get; set; } = Direction;
    }

    private const char OBSTRUCTION = '#';
    private readonly string _input;
    private readonly List<char> directions = new() { '^', '>', 'v', '<' };

    public Day06()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var grid = ParseInput(_input);

        // Find start position
        var start = (0, 0);
        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid[i].Length; j++)
            {
                if (directions.Contains(grid[i][j]))
                {
                    start = (i, j);
                    i = grid.Length;
                    break;
                }
            }
        }

        var visited = SimulateGuard(grid, new Guard(start, grid[start.Item1][start.Item2]));
        return new ValueTask<string>(visited.Visited.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var map = ParseInput(_input);
        var start = FindGuardStart(map, out var startDirection);
        var (_, visited, patrol) = SimulateGuard(map, new Guard(start, startDirection));
        patrol = patrol.Where(pos => pos.Position != start).ToList();

        var count = 0;

        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == '.')
                {
                    map[i][j] = '#';
                    var (inLoop, _, _) = SimulateGuard(map, new Guard(start, startDirection));
                    if (inLoop)
                    {
                        count++;
                    }
                    map[i][j] = '.';
                }
            }
        }

        return new ValueTask<string>(count.ToString());
    }

    private static char[][] ParseInput(string input)
    {
        var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var result = new char[lines.Length][];
        for (var i = 0; i < lines.Length; i++)
        {
            result[i] = lines[i].ToCharArray();
        }

        return result;
    }

    private (int, int) FindGuardStart(char[][] grid, out char startDirection)
    {
        for (var i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                if (directions.Contains(grid[i][j]))
                {
                    startDirection = grid[i][j];
                    return (i, j);
                }
            }
        }
        throw new InvalidOperationException("Guard start position not found");
    }

    private static (int, int) GetNextPosition((int, int) current, char direction)
    {
        return direction switch
        {
            '^' => (current.Item1 - 1, current.Item2),
            '>' => (current.Item1, current.Item2 + 1),
            'v' => (current.Item1 + 1, current.Item2),
            '<' => (current.Item1, current.Item2 - 1),
            _ => throw new InvalidOperationException("Invalid direction")
        };
    }

    private static char TurnRight(char direction)
    {
        return direction switch
        {
            '^' => '>',
            '>' => 'v',
            'v' => '<',
            '<' => '^',
            _ => throw new InvalidOperationException("Invalid direction")
        };
    }

    private static (bool InALoop, HashSet<(int, int)> Visited, List<Guard> patrol) SimulateGuard(char[][] map, Guard start)
    {
        var cache = new HashSet<Guard>();
        var patrol = new List<Guard>();
        var visited = new HashSet<(int, int)>();
        var position = start.Position;
        var direction = start.Direction;
        visited.Add(position);

        while (true)
        {
            if (cache.Add(new Guard(position, direction)) is false)
            {
                return (true, visited, patrol);
            }

            var nextPosition = GetNextPosition(position, direction);
            if (nextPosition.Item1 < 0 || nextPosition.Item1 >= map.Length ||
                nextPosition.Item2 < 0 || nextPosition.Item2 >= map[0].Length)
            {
                break;
            }

            if (map[nextPosition.Item1][nextPosition.Item2] == OBSTRUCTION)
            {
                direction = TurnRight(direction);
            }
            else
            {
                position = nextPosition;
                patrol.Add(new Guard(position, direction));
                visited.Add(position);
            }
        }
        return (false, visited, patrol);
    }

    private static int CountLoops(List<Guard> patrol, char[][] map)
    {
        HashSet<(int, int)> visited = new();
        int loops = 0;

        for (int i = 0; i < patrol.Count - 1; i++)
        {
            if (visited.Contains(patrol[i].Position))
            {
                continue;
            }

            visited.Add(patrol[i].Position);

            if (SimulateGuard(map, patrol[int.Clamp(i - 1, 0, patrol.Count - 2)]).InALoop)
            {
                loops++;
            }
        }

        return loops;
    }
}
