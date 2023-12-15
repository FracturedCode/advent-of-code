using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;
using MoreLinq;

namespace advent_2022;

public class Day12 : AdventDay
{
	public Day12() : base(nameof(Day12)) { }

	[Test(ExpectedResult = 31)]
	[Benchmark]
	public override object Part1()
	{
		int rowLength = Input.IndexOf('\n');
		var input = Input.Replace("\n", "");
		
		Dictionary<int, int> visited = new(input.Length);
		
		Queue<(int, int)> queue = new();
		int startNode = input.IndexOf('S');
		queue.Enqueue((startNode, 0));
		visited.Add(startNode, 0);

		int endDistance = 0;
		
		while (queue.TryDequeue(out var node))
		{
			foreach (var n in Neighbors(input, rowLength, node.Item1))
			{
				int nDistance = node.Item2 + 1;
				if (input[n] == 'E')
				{
					if (input[node.Item1] == 'z')
					{
						visited.Add(n, nDistance);
						endDistance = nDistance;
						queue.Clear();
						break;
					}
				}
				if (!visited.ContainsKey(n))
				{
					visited.Add(n, nDistance);
					queue.Enqueue((n, nDistance));
				}
			}
		}

		return endDistance;
	}

	private IEnumerable<int> Neighbors(string input, int rowLength, int node)
	{
		int row = node / rowLength;
		int column = node % rowLength;
		char nodeValue = input[node] == 'S' ? Char.MaxValue : input[node];

		var neighborOffsets = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };
		List<int> neighbors = new();
		neighborOffsets.ForEach(no =>
		{
			int nRow = row + no.Item1;
			int nColumn = column + no.Item2;

			if (nColumn >= 0 && nColumn < rowLength && nRow >= 0 && nRow < input.Length / rowLength)
			{
				int nIndex = nRow * rowLength + nColumn;
				if (input[nIndex] - 1 <= nodeValue)
				{
					neighbors.Add(nIndex);
				}
			}
		});
		return neighbors;
	}

	public override object Part2()
	{
		throw new NotImplementedException();
	}
}