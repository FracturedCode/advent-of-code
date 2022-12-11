using System.Diagnostics;
using MoreLinq;

namespace advent_2022;

public class Day9 : AdventDay
{
	public Day9() : base(nameof(Day9)) { }
	
	[Test(ExpectedResult = 88)]
	public override object Part1()
	{
		var currentHeadPosition = (0, 0);
		var currentTailPosition = (0, 0);
		HashSet<(int, int)> placesTailWent = new() { currentTailPosition };
		
		InputSplitByLine().ForEach(l =>
		{
			var direction = l[0] switch
			{
				'R' => (1, 0),
				'L' => (-1, 0),
				'U' => (0, 1),
				'D' => (0, -1),
				_ => throw new UnreachableException()
			};

			for (int i = 0; i < int.Parse(l[2..]); i++)
			{
				var lastHeadPosition = currentHeadPosition;
				currentHeadPosition = (lastHeadPosition.Item1 + direction.Item1,
					lastHeadPosition.Item2 + direction.Item2);
				int yDiff = currentHeadPosition.Item2 - currentTailPosition.Item2;
				int xDiff = currentHeadPosition.Item1 - currentTailPosition.Item1;
				if (Math.Sqrt(xDiff * xDiff + yDiff * yDiff) >= 2)
				{
					currentTailPosition = lastHeadPosition;
					placesTailWent.Add(currentTailPosition);
				}
			}
		});
		
		return placesTailWent.Count;
	}

	[Test(ExpectedResult = 36)]
	public override object Part2()
	{
		var currentPositions = new (int, int)[10];
		HashSet<(int, int)> placesTailWent = new() { currentPositions[^1] };

		InputSplitByLine().ForEach(l =>
		{
			var direction = l[0] switch
			{
				'R' => (0, -1),
				'L' => (0, 1),
				'U' => (1, 0),
				'D' => (-1, 0),
				_ => throw new UnreachableException()
			};
			var diagonals = new[] { (1, 1), (1, -1), (-1, 1), (-1, -1) };

			for (int i = 0; i < int.Parse(l[2..]); i++)
			{
				var lastKnotOldPosition = currentPositions[0];
				currentPositions[0] = (lastKnotOldPosition.Item1 + direction.Item1,
					lastKnotOldPosition.Item2 + direction.Item2);
				for (int j = 1; j < currentPositions.Length; j++)
				{
					var lastKnotNewPosition = currentPositions[j - 1];
					var thisKnot = currentPositions[j];

					int dx = lastKnotNewPosition.Item1 - thisKnot.Item1;
					int dy = lastKnotNewPosition.Item2 - thisKnot.Item2;
					if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
					{
						currentPositions[j].Item1 += Math.Sign(dx);
						currentPositions[j].Item2 += Math.Sign(dy);
						if (j == currentPositions.Length - 1)
						{
							placesTailWent.Add(currentPositions[j]);
						}
					}

					lastKnotOldPosition = thisKnot;
				}

				PrintPositions(currentPositions); // Only works with the example input
			}
		});

		return placesTailWent.Count;
	}

	private static double Distance((int, int) a, (int, int) b)
	{
		int xDiff = a.Item1 - b.Item1;
		int yDiff = a.Item2 - b.Item2;
		return Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
	}
	private static void PrintPositions((int, int)[] positions)
	{
		// This function only work with the example input
		var outputGrid = new char[21, 26];
		for (int i = 0; i < 21; i++)
		{
			for (int j = 0; j < 26; j++)
			{
				outputGrid[i, j] = '.';
			}
		}
		var zerozero = (15, 11);
		for (int i = positions.Length - 1; i >= 0; i--)
		{
			var pos = positions[i];
			outputGrid[zerozero.Item1 - pos.Item1, zerozero.Item2 - pos.Item2] = (char)(i + '0');
		}

		Console.WriteLine();
		for (int i = 0; i < 21; i++)
		{
			Console.WriteLine();
			for (int j = 0; j < 26; j++)
			{
				Console.Write(outputGrid[i, j]);
			}
		}
	}
}