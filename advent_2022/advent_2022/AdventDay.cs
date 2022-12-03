using Net.FracturedCode.Utilities;

namespace advent_2022;

public class AdventDay : GenericBenchmark
{
	public AdventDay(string inputName)
	{
		Input = File.ReadAllText($"inputs/{inputName}.txt");
	}
	[Test]
	public override void Benchmark() => DefaultBench();
	protected string Input { get; }
}