using BenchmarkDotNet.Attributes;
using Net.FracturedCode.Utilities;

namespace advent_2022;

public abstract class AdventDay : GenericBenchmark
{
	protected AdventDay(string inputName)
	{
		Input = File.ReadAllText($"inputs/{inputName}.txt");
	}
	[Test]
	public override void Benchmark() => DefaultBench();
	protected string Input { get; }

	protected string[] InputSplitByLine() =>
		Input.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

	public abstract int Part1();
	public abstract int Part2();
}