using BenchmarkDotNet.Attributes;

namespace advent_2022;

public class Day4 : AdventDay
{
	public Day4() : base(nameof(Day4)) { }

	[Test(ExpectedResult = 431)]
	[Benchmark]
	public override object Part1() =>
		ParseToTuples().Count(p => (p.left.lower >= p.right.lower && p.left.upper <= p.right.upper) ||
		               (p.right.lower >= p.left.lower && p.right.upper <= p.left.upper));

	[Test(ExpectedResult = 823)]
	[Benchmark]
	public override object Part2() =>
		// Thank you ChatGPT, it was 2am and I was too tired to think of all those boolean statements on my own.
		// Basically for each of the four bounds check if it is between the 2 bounds of the other range. 
		ParseToTuples().Count(p => p.left.lower >= p.right.lower && p.left.lower <= p.right.upper ||
		                                  p.left.upper >= p.right.lower && p.left.upper <= p.right.upper ||
		                                  p.right.lower >= p.left.lower && p.right.lower <= p.left.upper ||
		                                  p.right.upper >= p.left.lower && p.right.upper <= p.left.upper);

	private IEnumerable<((int lower, int upper) left, (int lower, int upper) right)> ParseToTuples() =>
		InputSplitByLine().Select(p =>
		{
			var sp = p.SplitDso(',').Select(sp => sp.SplitDso('-').Select(int.Parse).ToList()).ToList();
			return ((sp[0][0], sp[0][1]), (sp[1][0], sp[1][1]));
		});
}