import itertools
from typing import List, NamedTuple

from lib import check_and_submit

adjacents = [x for x in itertools.product([1, -1, 0], repeat=2) if not (x[0] == 0 and x[1] == 0)]

def get_adjacent_count(data, i, j) -> int:
    adj = map(lambda x: (i + x[0], j + x[1]), adjacents)
    return sum(map(lambda x: data[x[0]][x[1]] == "@" or data[x[0]][x[1]] == "x", adj))

def parse_and_pad(data: str) -> List[List[str]]:
    data = [x.strip() for x in data.strip().splitlines()]
    padding = "." * len(data[0])
    data = [padding] + data + [padding]
    return [list("." + x + ".") for x in data]

def remove_rolls(data: List[List[str]]) -> int:
    removed = 0
    for i in range(1, len(data) - 1):
        for j in range(1, len(data[0]) - 1):
            if data[i][j] == "@":
                is_accessible = get_adjacent_count(data, i, j) < 4
                removed += is_accessible
                if is_accessible:
                    data[i][j] = "x"

    for i in range(1, len(data) - 1):
        for j in range(1, len(data[0]) - 1):
            if data[i][j] == "x":
                data[i][j] = "."
    return removed

def solve_part_1_old(data: str) -> int:
    data = parse_and_pad(data)

    accessible_count = 0
    for i in range(1, len(data) - 1):
        for j in range(1, len(data[0]) - 1):
            if data[i][j] == "@":
                accessible_count += get_adjacent_count(data, i, j) < 4
    return accessible_count

def solve_part_1(data: str) -> int:
    data = parse_and_pad(data)
    return remove_rolls(data)

def solve_part_2(data: str) -> int:
    data = parse_and_pad(data)
    last_removed = 1
    removed = 0
    while last_removed > 0:
        last_removed = remove_rolls(data)
        removed += last_removed
    return removed


if __name__ == "__main__":
    check_and_submit(4, "a", solve_part_1)
    check_and_submit(4, "b", solve_part_2)