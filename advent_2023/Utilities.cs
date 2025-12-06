namespace advent_2023;

public static class Utilities
{
	// Default Split Options
	public static StringSplitOptions Dso => StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;

	public static string[] SplitDso(this string str, char splitChar) => str.Split(splitChar, Dso);
}