using System.Collections;
using BenchmarkDotNet.Attributes;

namespace advent_2022;

public class Day5 : AdventDay
{
	public Day5() : base(nameof(Day5)) { }

	[Test(ExpectedResult = 0)]
	[Benchmark]
	public override int Part1()
	{
		string ans = ManipulateStacks((from, to, moveCount) =>
		{
			for (int i = 0; i < moveCount; i++)
			{
				to.Push(from.Pop());
			}
		});

		Console.WriteLine(ans);
		return 0;
	}

	[Test(ExpectedResult = 0)]
	[Benchmark]
	public override int Part2()
	{
		string ans = ManipulateStacks((from, to, moveCount) =>
		{
			Stack<char> temp = new();
			for (int i = 0; i < moveCount; i++)
			{
				temp.Push(from.Pop());
			}

			for (int i = 0; i < moveCount; i++)
			{
				to.Push(temp.Pop());
			}
		});

		Console.WriteLine(ans);
		return 0;
	}

	private (string[], Stack<char>[]) ParseStacks()
	{
		string[] input = Input.Split('\n');
		var stacks = Enumerable.Range(0, 9).Select(_ => new Stack<char>()).ToArray();
		for (int j = 7; j >= 0; j--)
		{
			for (int i = 0; i < 9; i++)
			{
				char supply = input[j][4 * i + 1];
				if (!string.IsNullOrWhiteSpace(supply.ToString()))
					stacks[i].Push(supply);
			}
		}

		return (input, stacks);
	}

	private string ManipulateStacks(Action<Stack<char>, Stack<char>, int> moveFunc)
	{
		(string[] input, Stack<char>[] stacks) = ParseStacks();
		foreach (string ins in input.AsSpan(10..^1))
		{
			string[] sins = ins.SplitDso(' ');
			int moveCount = int.Parse(sins[1]);
			int from = int.Parse(sins[3]) - 1;
			int to = int.Parse(sins[5]) - 1;
			moveFunc(stacks[from], stacks[to], moveCount);
		}
		return string.Concat(stacks.Select(s => s.Peek()));
	}
}