using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using MoreLinq;

namespace advent_2022;

public class Day11 : AdventDay
{
	public Day11() : base(nameof(Day11)) { }

	[Test(ExpectedResult = 10605)]
	[Benchmark]
	public override object Part1()
	{
		var monkeys = InputSplitByLine().Chunk(6).Select(um => new Monkey(um[1][16..].Split(", ").Select(ulong.Parse),
			(um[2][21], !ulong.TryParse(um[2][(um[2].LastIndexOf(' ') + 1)..], out var operand), operand),
			(ulong.Parse(um[3][19..]), int.Parse(um[4][25..]), int.Parse(um[5][26..])))).ToList();
		
		for (int round = 1; round < 21; round++)
		{
			monkeys.ForEach((m, i) =>
			{
				while (m.Items.TryDequeue(out ulong item))
				{
					m.Worry = item;
					m.Worry = (ulong)Math.Floor(m.Operation(m.Worry) / 3.0);
					int recipientMonkey = m.Test(m.Worry);
					monkeys[recipientMonkey].Items.Enqueue(m.Worry);
					m.InspectedItemCount++;
				}
			});
		}

		return monkeys.Select(m => m.InspectedItemCount).OrderDescending().Take(2).Aggregate((x, y) => x * y);
	}

	[Test(ExpectedResult = 2713310158)]
	[Benchmark]
	public override object Part2()
	{
		var monkeys = InputSplitByLine().Chunk(6).Select(um => new Monkey(um[1][16..].Split(", ").Select(ulong.Parse),
			(um[2][21], !ulong.TryParse(um[2][(um[2].LastIndexOf(' ') + 1)..], out var operand), operand),
			(ulong.Parse(um[3][19..]), int.Parse(um[4][25..]), int.Parse(um[5][26..])))).ToList();

		var factor = monkeys.Select(m => m.Divisor).Aggregate((x, y) => x * y);
		
		for (int round = 1; round < 10001; round++)
		{
			monkeys.ForEach((m, i) =>
			{
				while (m.Items.TryDequeue(out ulong item))
				{
					m.Worry = item;
					m.Worry = m.Operation(m.Worry);
					m.Worry %= factor;
					int recipientMonkey = m.Test(m.Worry);
					monkeys[recipientMonkey].Items.Enqueue(m.Worry);
					m.InspectedItemCount++;
				}
			});
		}

		return monkeys.Select(m => m.InspectedItemCount).OrderDescending().Take(2).Aggregate((x, y) => x * y);
	}

	private class Monkey
	{
		public Monkey(IEnumerable<ulong> items,
			(char operationType, bool worryOperand, ulong operand) operation,
			(ulong operand, int trueMonkey, int falseMonkey) test)
		{
			Items = new(items);
			Operation = operation.operationType switch
			{
				'+' => worry => worry + (operation.worryOperand ? worry : operation.operand),
				'*' => worry => worry * (operation.worryOperand ? worry : operation.operand),
				_ => throw new UnreachableException()
			};
			Divisor = test.operand;
			Test = worry => worry % Divisor == 0 ? test.trueMonkey : test.falseMonkey;
		}

		public ulong Divisor { get; }
		public ulong InspectedItemCount { get; set; }
		public ulong Worry { get; set; }
		public Queue<ulong> Items { get; }
		public Func<ulong, ulong> Operation { get; }
		public Func<ulong, int> Test { get; }
	}
}