using System.Diagnostics;
using System.Text;
using BenchmarkDotNet.Attributes;
using MoreLinq;

namespace advent_2022;

[TestFixture]
public class Day10 : AdventDay
{
	public Day10() : base(nameof(Day10)) { }

	[Test(ExpectedResult = 13140)]
	[Benchmark]
	public override object Part1()
	{
		int X = 1;
		int signalStrengthSum = 0;
		var input = InputSplitByLine().GetEnumerator();

		for (int clock = 1; clock < int.MaxValue; clock++)
		{
			if (clock is 20 or 60 or 100 or 140 or 180 or 220)
			{
				signalStrengthSum += clock * X;
			}
			if (!input.MoveNext())
			{
				break;
			}
			if (((string)input.Current)[0] == 'a')
			{
				clock++;
				if (clock is 20 or 60 or 100 or 140 or 180 or 220)
				{
					signalStrengthSum += clock * X;
				}
				X += int.Parse(((string)input.Current)[5..]);
			}
		}

		return signalStrengthSum;
	}
	[Test(ExpectedResult = """
##..##..##..##..##..##..##..##..##..##..
###...###...###...###...###...###...###.
####....####....####....####....####....
#####.....#####.....#####.....#####.....
######......######......######......####
#######.......#######.......#######.....
"""
	)]
	[Benchmark]
	public override object Part2()
	{
		int X = 1;
		var program = InputSplitByLine().GetEnumerator();
		program.MoveNext();
		const int clkStart = 1;
		(string, int) currentInstruction = ((string)program.Current, clkStart);
		StringBuilder sb = new();
		
		for (int clk = clkStart; clk < 241; clk++)
		{
			// Get next instruction if necessary
			if ((currentInstruction.Item1[0] == 'a' && clk - currentInstruction.Item2 == 2) ||
			    currentInstruction.Item1[0] == 'n')
			{
				program.MoveNext();
				currentInstruction = ((string)program.Current, clk);
			}
			
			// Render CRT
			int hSpritePos = X % 40;
			int hPixel = (clk - 1) % 40;
			char value = Math.Abs(hSpritePos - hPixel) <= 1 ? '#' : '.';
			sb.Append(value);
			
			// Complete operation if possible
			if (currentInstruction.Item1[0] == 'a' && clk - currentInstruction.Item2 == 1)
			{
				X += int.Parse(currentInstruction.Item1[5..]);
			}
		}

		string output = sb.ToString()
			.Chunk(40)
			.Select(x => new string(x))
			.ToDelimitedString("\n");
		Debug.WriteLine(output);
		return output;
	}
}