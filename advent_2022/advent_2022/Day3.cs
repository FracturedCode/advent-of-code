using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using MoreLinq;

namespace advent_2022;

public class Day3 : AdventDay
{
	public Day3() : base(nameof(Day3)) { }

	[Test(ExpectedResult = 7568)]
	[Benchmark]
	public override object Part1() => GetPrioritySum(
		InputSplitByLine().Select(line =>
			{
				int halfLength = line.Length / 2;
				ReadOnlySpan<char> compartment1 = line.AsSpan(0, halfLength);
				ReadOnlySpan<char> compartment2 = line.AsSpan(halfLength, halfLength);

				char commonItem;
				foreach (char i in compartment1)
				{
					foreach (char j in compartment2)
					{
						if (i == j)
						{
							commonItem = i;
							goto afterForeach;
						}
					}
				}
				throw new UnreachableException();
				
				afterForeach:
				return commonItem;
			}));

	[Test(ExpectedResult = 2780)]
	[Benchmark]
	public override object Part2() => GetPrioritySum(
		InputSplitByLine().Chunk(3).Select(g =>
			{
				char commonItem;
				foreach (char i in g[0])
				{
					foreach (char j in g[1])
					{
						if (i == j)
						{
							commonItem = i;
							foreach (char k in g[2])
							{
								if (k == commonItem)
								{
									goto afterForEach;
								}
							}
						}
					}
				}

				throw new UnreachableException();
				
				afterForEach:
				return commonItem;
			}));

	private static int GetPrioritySum(IEnumerable<char> items) => items.Sum(i => i - (i <= 'Z' ? 38 : 96));
}