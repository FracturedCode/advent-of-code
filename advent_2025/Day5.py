from typing import List
from lib import check_and_submit, Range

def is_fresh(ingredient: int, ranges: List[Range]) -> bool:
    return any(filter(lambda x: x.start <= ingredient <= x.stop_inclusive, ranges))

def parse_ranges(data: str) -> List[Range]:
    ranges_end = data.index("\n\n")
    ranges = data[:ranges_end].splitlines()
    ranges = [x.strip().split('-') for x in ranges]
    return [Range(int(x[0]), int(x[1])) for x in ranges]

def solve_part_1(data: str) -> int:
    ranges = parse_ranges(data)
    ranges_end = data.index("\n\n")
    ingredients = [int(x) for x in data[ranges_end+2:].splitlines()]

    return sum(map(lambda x: is_fresh(x, ranges), ingredients))

def solve_part_2(data: str) -> int:
    ranges = parse_ranges(data)
    ranges.sort(key=lambda x: x.start)
    merged_ranges = []
    merged_start = -1
    merged_stop_inclusive = -1
    for start, stop_inclusive in ranges:
        if start > merged_stop_inclusive:
            merged_ranges.append(Range(start, stop_inclusive))
            merged_start, merged_stop_inclusive = start, stop_inclusive
        else:
            merged_stop_inclusive = max(stop_inclusive, merged_stop_inclusive)
            merged_ranges[-1] = Range(merged_start, merged_stop_inclusive)

    return sum(map(lambda x: x.stop_inclusive - x.start + 1, merged_ranges))

if __name__ == "__main__":
    check_and_submit(5, "a", solve_part_1)
    check_and_submit(5, "b", solve_part_2)