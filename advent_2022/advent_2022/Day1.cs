using System.IO.Pipes;
using System.Runtime.InteropServices.JavaScript;
using BenchmarkDotNet.Attributes;

namespace advent_2022;

public class Day1 : Advent
{
	[Test(ExpectedResult = 72602)]
	[Benchmark]
	public int Part1Linq() => ParsedSums().Max();

	[Test(ExpectedResult = 207410)]
	[Benchmark]
	public int Part2Linq() => ParsedSums().OrderDescending().Take(3).Sum();

	private IEnumerable<int> ParsedSums() =>
		Input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
			.Select(i => i.Split('\n').Sum(int.Parse));
}