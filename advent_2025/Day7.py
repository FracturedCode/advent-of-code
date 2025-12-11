from functools import cache
from typing import List
from lib import check_and_submit

# Solution using dfs

def parse_data(data: str) -> List[List[str]]:
    data = data.splitlines()
    data[0] = data[0].replace("S", "|")
    return [list(x) for x in data]

def solve_part_1(data: str) -> int:
    data = parse_data(data)

    pw = len(data[0])
    split_count = 0
    for i in range(1, len(data)):
        prev = data[i - 1]
        curr = data[i]
        for j in range(pw):
            if prev[j] == "|" and curr[j] == ".":
                curr[j] = "|"
            elif prev[j] == "|" and curr[j] == "^":
                split_count += 1
                if curr[j - 1] != "^":
                    curr[j - 1] = "|"
                if curr[j + 1] != "^":
                    curr[j + 1] = "|"

    return split_count

def solve_part_2(data: str) -> int:
    data = parse_data(data)

    @cache
    def dfs(i: int, j: int) -> int:
        while data[i][j] == ".":
            i += 1
            if i == len(data):
                return 1
        return dfs(i, j - 1) + dfs(i, j + 1)

    return dfs(1, data[0].index("|"))


if __name__ == "__main__":
    check_and_submit(7, "a", solve_part_1)
    check_and_submit(7, "b", solve_part_2)