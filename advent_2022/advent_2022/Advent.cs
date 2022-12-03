using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Net.FracturedCode.Utilities;

namespace advent_2022;

public class Advent : GenericBenchmark
{
	[Test]
	public override void Benchmark() => DefaultBench();
	protected string Input { get; private set; }
	
	protected object Answer { get; set; }

	[SetUp]
	[GlobalSetup]
	public void SetInput() => Input = File.ReadAllText($"inputs/{GetType().Name}.txt");
}