using Common;
using System.Data;
using System.Text.RegularExpressions;

namespace AoC_2024;

public class Day09 : Base2024Day
{
    private readonly string _input;

    public Day09()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<(int length, int id)> blocks = new();
        int fileId = 0;

        for (int i = 0; i < _input.Length; i += 2)
        {
            int fileLength = _input[i] - '0';
            int emptyLength = _input[i + 1] - '0';

            if (fileLength > 0)
            {
                blocks.Add((fileLength, fileId++));
            }
            if (emptyLength > 0)
            {
                blocks.Add((emptyLength, -1));
            }
        }

        // Compact - Move one item at a time to the left
        List<int> disk = new();
        foreach (var (length, id) in blocks)
        {
            for (int i = 0; i < length; i++)
            {
                disk.Add(id);
            }
        }

        List<int> newDisk = disk;

        int left = 0;
        int right = newDisk.Count - 1;

        while (left < right)
        {
            if (newDisk[left] == -1)
            {
                if (newDisk[right] == -1)
                {
                    right--;
                    continue;
                }

                newDisk[left] = newDisk[right];
                newDisk[right] = -1;

                left++;
                right--;
            }

            if (newDisk[left] != -1)
            {
                left++;
            }
        }

        // Calculate checksum
        long checksum = 0;
        for (int i = 0; i < newDisk.Count; i++)
        {
            if (newDisk[i] != -1)
            {
                checksum += i * newDisk[i];
            }
        }

        return new ValueTask<string>(checksum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        List<(int length, int id)> blocks = new();
        int fileId = 0;

        for (int i = 0; i < _input.Length; i += 2)
        {
            int fileLength = _input[i] - '0';
            int emptyLength = _input[i + 1] - '0';

            if (fileLength > 0)
            {
                blocks.Add((fileLength, fileId++));
            }
            if (emptyLength > 0)
            {
                blocks.Add((emptyLength, -1));
            }
        }

        List<int> disk = new();
        foreach (var (length, id) in blocks)
        {
            for (int i = 0; i < length; i++)
            {
                disk.Add(id);
            }
        }

        List<int> newDisk = disk;

        // Identify files and their positions
        var files = new List<(int id, int start, int length)>();
        int currentId = -1;
        int currentStart = -1;
        int currentLength = 0;

        for (int i = 0; i < newDisk.Count; i++)
        {
            if (newDisk[i] != -1)
            {
                if (newDisk[i] != currentId)
                {
                    if (currentId != -1)
                    {
                        files.Add((currentId, currentStart, currentLength));
                    }
                    currentId = newDisk[i];
                    currentStart = i;
                    currentLength = 1;
                }
                else
                {
                    currentLength++;
                }
            }
            else if (currentId != -1)
            {
                files.Add((currentId, currentStart, currentLength));
                currentId = -1;
                currentStart = -1;
                currentLength = 0;
            }
        }
        if (currentId != -1)
        {
            files.Add((currentId, currentStart, currentLength));
        }

        // Sort files by ID in descending order
        files.Sort((a, b) => b.id.CompareTo(a.id));

        // Move files to the leftmost span of free space
        foreach (var file in files)
        {
            int left = 0;
            while (left < file.start)
            {
                int freeSpaceLength = 0;
                while (left < file.start && newDisk[left] == -1)
                {
                    freeSpaceLength++;
                    left++;
                }

                if (freeSpaceLength >= file.length)
                {
                    for (int i = 0; i < file.length; i++)
                    {
                        newDisk[left - freeSpaceLength + i] = file.id;
                        newDisk[file.start + i] = -1;
                    }
                    break;
                }

                while (left < file.start && newDisk[left] != -1)
                {
                    left++;
                }
            }
        }

        // Calculate checksum
        long checksum = 0;
        for (int i = 0; i < newDisk.Count; i++)
        {
            if (newDisk[i] != -1)
            {
                checksum += i * newDisk[i];
            }
        }

        return new ValueTask<string>(checksum.ToString());
    }
}
