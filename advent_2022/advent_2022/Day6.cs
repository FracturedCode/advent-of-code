using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters;

namespace advent_2022;

[MemoryDiagnoser]
[MinColumn]
[MaxAbsoluteError(.15)]
public class Day6 : AdventDay
{
	public Day6() : base(nameof(Day6)) { }

	[Test]
	public new void Benchmark() => DefaultBench(CustomDefaultConfig.AddExporter(RPlotExporter.Default));

	[Test(ExpectedResult = 1238)]
	[Benchmark]
	public override object Part1()
	{
		for (int i = 3; i < Input.Length; i++)
		{
			if (! (Input[i - 3] == Input[i - 2] || Input[i - 3] == Input[i - 1] || Input[i - 3] == Input[i]
			    || Input[i - 2] == Input[i - 1] || Input[i - 2] == Input[i]
			    || Input[i - 1] == Input[i]))
			{
				return i + 1;
			}
		}

		throw new UnreachableException();
	}
	
	[Test(ExpectedResult = 3037)]
	[Benchmark]
	public override object Part2()
	{
		var input = Input.AsSpan();
		for (int i = 13; i < input.Length; i++)
		{
			bool isStart = true;
			for (int j = 13; j > 0; j--)
			{
				int compareIndex = i - j;
				if (input.Slice(compareIndex + 1, j).Contains(input[compareIndex]))
				{
					isStart = false;
					break;
				}
			}
			if (isStart)
				return i + 1;
		}

		throw new UnreachableException();
	}

	[Test(ExpectedResult = 3037)]
	[Benchmark]
	public int Part2Oldest()
	{
		for (int i = 13; i < Input.Length; i++)
		{
			bool isStart = true;
			for (int j = 13; j > 0; j--)
			{
				int compareIndex = i - j;
				if (Input.AsSpan(compareIndex + 1, j).Contains(Input[compareIndex]))
				{
					isStart = false;
					break;
				}
			}
			if (isStart)
				return i + 1;
		}

		throw new UnreachableException();
	}

	[Test(ExpectedResult = 3037)]
	[Benchmark]
	public int Part2AlwaysWasSpan()
	{
		var input = Input.AsSpan();
		for (int i = 13; i < Input.Length; i++)
		{
			bool isStart = true;
			for (int j = 13; j > 0; j--)
			{
				int compareIndex = i - j;
				if (input.Slice(compareIndex + 1, j).Contains(Input[compareIndex]))
				{
					isStart = false;
					break;
				}
			}
			if (isStart)
				return i + 1;
		}

		throw new UnreachableException();
	}
	
	[Test(ExpectedResult = 3037)]
	[Benchmark]
	public int Part2SpanContains()
	{
		var input = Input.AsSpan();
		for (int i = 13; i < Input.Length; i++)
		{
			bool isStart = true;
			for (int j = 13; j > 0; j--)
			{
				int compareIndex = i - j;
				if (input.Slice(compareIndex + 1, j).Contains(input[compareIndex]))
				{
					isStart = false;
					break;
				}
			}
			if (isStart)
				return i + 1;
		}

		throw new UnreachableException();
	}

	// I was like, "Eh, what if I didn't used the in-built .Contains()?
	// That probably has a bunch of scaffolding and stuff to make it generic to a wide range of types."
	// Well, it does, but it also uses bitmasks and hardware acceleration.
	// I probably *could* make it faster, with difficulty, but I'm not really interested atm.
	// This code is 10x slower than using .Contains()
	[Test(ExpectedResult = 3037)]
	[Benchmark]
	public int Part2CustomContainsForeach()
	{
		var input = Input.AsSpan();
		for (int i = 13; i < input.Length; i++)
		{
			bool isStart = true;
			for (int j = 13; j > 0; j--)
			{
				int compareIndex = i - j;
				foreach (char x in input.Slice(compareIndex + 1, j))
				{
					if (x == input[compareIndex])
					{
						isStart = false;
						break;
					}
				}
				
			}
			if (isStart)
				return i + 1;
		}

		throw new UnreachableException();
	}
}