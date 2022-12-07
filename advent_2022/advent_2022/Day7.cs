using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using static MoreLinq.Extensions.ToDelimitedStringExtension;

namespace advent_2022;

public class Day7 : AdventDay
{
	public Day7() : base(nameof(Day7)) { }

	[Test(ExpectedResult = 1477771)]
	[Benchmark]
	public override object Part1() =>
		ConstructDirectories().Where(d => d.Size <= 100000).Sum(d => d.Size);

	[Test(ExpectedResult = 3579501)]
	[Benchmark]
	public override object Part2() => DoPart2(ConstructDirectories());

	[Test(ExpectedResult = 1477771)]
	[Benchmark]
	public int Part1Switches() =>
		ConstructDirectoriesSwitches().Where(d => d.Size <= 100000).Sum(d => d.Size);

	[Test(ExpectedResult = 3579501)]
	[Benchmark]
	public int Part2Switches() =>
		DoPart2(ConstructDirectoriesSwitches());

	private static int DoPart2(List<Directory> allDirs)
	{
		int spaceUsed = allDirs.Single(d => d.Name == string.Empty).Size;
		const int totalSpace = 70000000;
		int unusedSpace = totalSpace - spaceUsed;
		const int requiredSpace = 30000000;
		int minimumFolderDeleteSize = requiredSpace - unusedSpace;
		if (minimumFolderDeleteSize < 0)
			throw new Exception(
				$"Directory space calculation must be incorrect because {nameof(minimumFolderDeleteSize)} is less than 0");
		return allDirs.Where(d => d.Size >= minimumFolderDeleteSize).Min(d => d.Size);
	}

	private List<Directory> ConstructDirectories()
	{
		CurrentWorkingDirectory workingDir = new();
		List<Directory> directoryOfDirectories = new() { workingDir.LastDir };
		foreach (string[] x in InputSplitByLine().Select(x => x.SplitDso(' ')))
		{
			string indicator = x[0];
			if (indicator == "$")
			{
				string command = x[1];
				if (command == "cd")
				{
					string argument = x[2];
					if (argument == "/")
					{
						workingDir.Reset();
					} else if (argument == "..")
					{
						workingDir.Up();
					}
					else
					{
						workingDir.Into(argument);
					}
				} 
				else if (command == "ls")
				{
					
				}
				else
				{
					throw new UnreachableException();
				}
			}
			else if (indicator == "dir")
			{
				Directory newDir = new(x[1]);
				workingDir.LastDir.AddDirectory(newDir);
				directoryOfDirectories.Add(newDir);
			}
			else if (int.TryParse(indicator, out int size))
			{
				workingDir.LastDir.AddFile(new AdventFile(x[1], size));
			}
			else
			{
				throw new UnreachableException();
			}
		}

		return directoryOfDirectories;
	}
	
	private List<Directory> ConstructDirectoriesSwitches()
	{
		CurrentWorkingDirectory workingDir = new();
		List<Directory> directoryOfDirectories = new() { workingDir.LastDir };
		foreach (string[] x in InputSplitByLine().Select(x => x.SplitDso(' ')))
		{
			string indicator = x[0];
			switch (indicator)
			{
				case "$":
				{
					string command = x[1];
					switch (command)
					{
						case "cd":
						{
							string argument = x[2];
							switch (argument)
							{
								case "/":
									workingDir.Reset();
									break;
								case "..":
									workingDir.Up();
									break;
								default:
									workingDir.Into(argument);
									break;
							}

							break;
						}
						case "ls":
							break;
						default:
							throw new UnreachableException();
					}

					break;
				}
				case "dir":
				{
					Directory newDir = new(x[1]);
					workingDir.LastDir.AddDirectory(newDir);
					directoryOfDirectories.Add(newDir);
					break;
				}
				default:
				{
					if (int.TryParse(indicator, out int size))
					{
						workingDir.LastDir.AddFile(new AdventFile(x[1], size));
					}
					else
					{
						throw new UnreachableException();
					}

					break;
				}
			}
		}

		return directoryOfDirectories;
	}

	private class CurrentWorkingDirectory : Stack<Directory>
	{
		public CurrentWorkingDirectory()
		{
			Push(new Directory(string.Empty));
		}

		public Directory LastDir => Peek();

		public void Reset()
		{
			Directory root = this.First();
			Clear();
			Push(root);
		}

		public void Up() => Pop();

		public void Into(string childName) =>
			Push(LastDir.GetChildDir(childName));
		
		public override string ToString() => "/" + this.Select(d => d.Name).ToDelimitedString("/");
	}
	
	private record Directory(string Name)
	{
		private List<Directory> ChildDirs { get; } = new();
		private List<AdventFile> Files { get; } = new();

		private int? _size;

		public int Size => _size ??= Files.Sum(f => f.Size) + ChildDirs.Sum(d => d.Size);

		public void AddDirectory(Directory dir)
		{
			if (ChildDirs.Any(d => d.Name == dir.Name))
				throw new Exception("Duplicate folder");
			ChildDirs.Add(dir);
		}
		public void AddFile(AdventFile file)
		{
			if (Files.Any(f => f.Name == file.Name))
				throw new Exception("Duplicate file");
			Files.Add(file);
		}

		public Directory GetChildDir(string childName) =>
			ChildDirs.Single(d => d.Name == childName);
	}

	private record AdventFile(string Name, int Size);
}