namespace AdventOfCode;

public class Day05 : BaseDay
{
    private readonly string[] _input;
    List<long> seeds;
    List<List<(long source, long destination, long length)>> maps;

    public Day05()
    {
        _input = File.ReadAllLines(InputFilePath);
    
        seeds = _input[0].Split(' ').Skip(1).Select(x => long.Parse(x)).ToList();
        maps = new List<List<(long source, long destination, long length)>>();
        List<(long source, long destination, long length)> currentMap = null;

        foreach (var line in _input.Skip(2))
        {
            if (line.EndsWith(':')) // Start of a new map
            {
                currentMap = new List<(long source, long destination, long length)>();
                continue;
            }
            else if (line.Length == 0 && currentMap != null) // End of a map
            {
                maps.Add(currentMap);
                currentMap = null;
                continue;
            }

            var nums = line.Split(' ').Select(x => long.Parse(x)).ToList();
            currentMap.Add((nums[1], nums[1] + nums[2] - 1, nums[0] - nums[1]));
        }

        if (currentMap != null)
        {
            maps.Add(currentMap);
        }
    }

    public override ValueTask<string> Solve_1()
    {
        long result = long.MaxValue;
        foreach (var seed in seeds)
        {
            var value = seed;
            foreach (var map in maps)
            {
                foreach (var item in map)
                {
                    if (value >= item.source && value <= item.destination)
                    {
                        value += item.length;
                        break;
                    }
                }
            }
            result = Math.Min(result, value);
        }      

        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var ranges = new List<(long source, long destination)>();
        for (int i = 0; i < seeds.Count; i += 2)
            ranges.Add((seeds[i], seeds[i] + seeds[i + 1] - 1));

        foreach (var map in maps)
        {
            var mapSorted = map.OrderBy(x => x.source).ToList();

            var newranges = new List<(long from, long to)>();
            foreach (var _range in ranges)
            {
                var range = _range;
                foreach (var mapSortedVal in mapSorted)
                {
                    if (range.source < mapSortedVal.source)
                    {
                        newranges.Add(
                            (
                                range.source, // from
                                Math.Min(range.destination, mapSortedVal.source - 1) // to
                            ));
                        range.source = mapSortedVal.source;

                        if (range.source > range.destination) break;
                    }

                    if (range.source <= mapSortedVal.destination)
                    {
                        newranges.Add(
                            (
                                range.source + mapSortedVal.length, // from
                                Math.Min(range.source, mapSortedVal.source) + mapSortedVal.length // to
                            ));
                        range.source = mapSortedVal.destination + 1;

                        if (range.source > range.destination) break;
                    }
                }
                if (range.source <= range.destination)
                {
                    newranges.Add(range);
                }
            }
            ranges = newranges;
        }

        return new ValueTask<string>($"{ranges.Min(r => r.source)}");        
    }
}

