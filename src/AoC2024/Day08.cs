using Common;
using System.Data;
using System.Text.RegularExpressions;

namespace AoC_2024;

public class Day08 : Base2024Day
{
    private readonly string _input;

    public Day08()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var lines = _input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var antennas = new Dictionary<char, List<(int x, int y)>>();

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                char c = lines[y][x];
                if (char.IsLetterOrDigit(c))
                {
                    if (!antennas.ContainsKey(c))
                    {
                        antennas[c] = new List<(int x, int y)>();
                    }
                    antennas[c].Add((x, y));
                }
            }
        }

        var antinodes = new HashSet<(int x, int y)>();

        foreach (var antenna in antennas)
        {
            var positions = antenna.Value;
            for (int i = 0; i < positions.Count; i++)
            {
                for (int j = i + 1; j < positions.Count; j++)
                {
                    var (x1, y1) = positions[i];
                    var (x2, y2) = positions[j];

                    // Calculate potential antinode positions
                    int dx = x2 - x1;
                    int dy = y2 - y1;

                    // Antinode positions
                    var antinode1 = (x1 - dx, y1 - dy);
                    var antinode2 = (x2 + dx, y2 + dy);

                    // Check if within bounds
                    if (IsWithinBounds(antinode1, lines.Length, lines[0].Length))
                    {
                        antinodes.Add(antinode1);
                    }
                    if (IsWithinBounds(antinode2, lines.Length, lines[0].Length))
                    {
                        antinodes.Add(antinode2);
                    }
                }
            }
        }

        return new ValueTask<string>(antinodes.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var lines = _input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var antennas = new Dictionary<char, List<(int x, int y)>>();

        // Parse the input map
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                char c = lines[y][x];
                if (char.IsLetterOrDigit(c))
                {
                    if (!antennas.ContainsKey(c))
                    {
                        antennas[c] = new List<(int x, int y)>();
                    }
                    antennas[c].Add((x, y));
                }
            }
        }

        var antinodes = new HashSet<(int x, int y)>();

        foreach (var antenna in antennas)
        {
            var positions = antenna.Value;
            if (positions.Count > 1)
            {
                // Include each antenna position as an antinode
                foreach (var pos in positions)
                {
                    antinodes.Add(pos);
                }

                for (int i = 0; i < positions.Count; i++)
                {
                    for (int j = i + 1; j < positions.Count; j++)
                    {
                        var (x1, y1) = positions[i];
                        var (x2, y2) = positions[j];

                        // Calculate potential antinode positions
                        int dx = x2 - x1;
                        int dy = y2 - y1;

                        // Antinode positions
                        for (int k = 1; k <= Math.Max(lines.Length, lines[0].Length); k++)
                        {
                            var antinode1 = (x1 - k * dx, y1 - k * dy);
                            var antinode2 = (x2 + k * dx, y2 + k * dy);

                            // Check if within bounds
                            if (IsWithinBounds(antinode1, lines.Length, lines[0].Length))
                            {
                                antinodes.Add(antinode1);
                            }
                            if (IsWithinBounds(antinode2, lines.Length, lines[0].Length))
                            {
                                antinodes.Add(antinode2);
                            }
                        }
                    }
                }
            }
        }

        return new ValueTask<string>(antinodes.Count.ToString());
    }

    private bool IsWithinBounds((int x, int y) position, int height, int width)
    {
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    }
}
