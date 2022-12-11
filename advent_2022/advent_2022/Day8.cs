using BenchmarkDotNet.Attributes;
using MoreLinq.Extensions;

namespace advent_2022;

public class Day8 : AdventDay
{
	public Day8() : base(nameof(Day8)) { }

	[Test(ExpectedResult = 21)]
	[Benchmark]
	public override object Part1()
	{
		var input = InputSplitByLine().Select(r => r.Select(t => t - '0').ToArray()).ToArray();
		
		int rowLength = input[0].Length;
		var visibilityGrid = new bool[input.Length, rowLength];

		for (int i = 1; i < input.Length - 1; i++)
		{
			int lastMax = input[i][0];
			
			for (int j = 1; j < rowLength - 1; j++) // left-right
			{
				if (lastMax == 9)
				{
					break;
				}
				int value = input[i][j];
				if (value > lastMax)
				{
					lastMax = value;
					visibilityGrid[i, j] = true;
				}
			}

			lastMax = input[i][^1];
			for (int j = rowLength - 2; j > 0; j--) // right-left
			{
				if (lastMax == 9)
				{
					break;
				}
				int value = input[i][j];
				if (value > lastMax)
				{
					lastMax = value;
					visibilityGrid[i, j] = true;
				}
			}
		}

		for (int j = 1; j < rowLength - 1; j++)
		{
			int lastMax = input[0][j];
			for (int i = 0; i < input.Length - 1; i++) // top-bottom
			{
				if (lastMax == 9)
				{
					break;
				}
				int value = input[i][j];
				if (value > lastMax)
				{
					lastMax = value;
					visibilityGrid[i, j] = true;
				}
			}

			lastMax = input[^1][j];
			for (int i = input.Length - 2; i > 0; i--) // bottom-top
			{
				if (lastMax == 9)
				{
					break;
				}
				int value = input[i][j];
				if (value > lastMax)
				{
					lastMax = value;
					visibilityGrid[i, j] = true;
				}
			}
		}

		return visibilityGrid.Flatten().Cast<bool>().Count(t => t) + (rowLength - 2) * 2 + input.Length * 2;
	}

	[Test(ExpectedResult = 8)]
	[Benchmark]
	public override object Part2()
	{
		var input = InputSplitByLine().Select(r => r.Select(t => t - '0').ToArray()).ToArray();
		int rowLength = input[0].Length;
		var scenicGrid = new int[input.Length, rowLength];
		for (int i = 0; i < input.Length; i++)
		{
			for (int j = 0; j < rowLength; j++)
			{
				scenicGrid[i, j] = 1;
			}
		}
		var directions = new[] { (1, 0), (-1, 0), (0, 1), (0, -1) };

		for (int i = 1; i < input.Length - 1; i++)
		{
			for (int j = 1; j < rowLength - 1; j++)
			{
				int currentTree = input[i][j];
				foreach (var dir in directions)
				{
					int k = 0;
					while (true)
					{
						k++;
						int ciindex = k * dir.Item1 + i;
						int cjindex = k * dir.Item2 + j;
						if (ciindex < 0 || ciindex > input.Length - 1 || cjindex < 0 || cjindex > rowLength - 1)
						{
							k--;
							break;
						}
						if (input[ciindex][cjindex] >= currentTree)
						{
							break;
						}
					}

					scenicGrid[i, j] *= k;
				}
			}
		}

		int max = 0;
		for (int i = 0; i < input.Length; i++)
		{
			for (int j = 0; j < rowLength; j++)
			{
				if (scenicGrid[i, j] > max)
				{
					max = scenicGrid[i, j];
				}
			}
		}

		return max;
	}
}