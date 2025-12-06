using BenchmarkDotNet.Exporters;
using Net.FracturedCode.Utilities;

namespace advent_2023;

public abstract class AdventDay : GenericBenchmark
{
	protected AdventDay(string inputName)
	{
		Input = File.ReadAllText($"inputs/{inputName}.txt");
	}
	[TestMethod]
	public override void Benchmark() => DefaultBench(CustomDefaultConfig.AddExporter(RPlotExporter.Default));
	protected string Input { get; }

	protected string[] InputSplitByLine() =>
		Input.SplitDso('\n');

	public abstract object Part1();
	public abstract object Part2();
}