using System.Diagnostics;
using BenchmarkDotNet.Attributes;

namespace advent_2022;

public class Day2 : AdventDay
{
	public Day2() : base(nameof(Day2)) { }

	[Test(ExpectedResult = 11841)]
	[Benchmark]
	public override int Part1() => SumFromSwitch(r => r switch
	{
		"A X" => 4,
		"A Y" => 8,
		"A Z" => 3,
		"B X" => 1,
		"B Y" => 5,
		"B Z" => 9,
		"C X" => 7,
		"C Y" => 2,
		"C Z" => 6,
		_ => throw new UnreachableException()
	});

	[Test(ExpectedResult = 13022)]
	[Benchmark]
	public override int Part2() => SumFromSwitch(r => r switch
	{
		"A X" => 3,
		"A Y" => 4,
		"A Z" => 8,
		"B X" => 1,
		"B Y" => 5,
		"B Z" => 9,
		"C X" => 2,
		"C Y" => 6,
		"C Z" => 7,
		_ => throw new UnreachableException()
	});

	private int SumFromSwitch(Func<string, int> selector) => InputSplitByLine().Sum(selector);
}