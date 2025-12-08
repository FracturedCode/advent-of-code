from typing_extensions import deprecated

from lib import check_and_submit

@deprecated("Old part a solution")
def joltage_old(bank: str) -> int:
    first = max(bank[:-1])
    fi = bank.index(first)
    second = max(bank[fi + 1:])
    return int(first + second)

def joltage(bank: str, batts_left: int) -> str:
    if batts_left == 1:
        return max(bank)

    batt = max(bank[:-(batts_left - 1)])
    batt_idx = bank.index(batt)
    return batt + joltage(bank[batt_idx + 1:], batts_left - 1)

def solve(data: str, batts: int) -> int:
    data = [x.strip() for x in data.splitlines()]
    return sum(map(lambda x: int(joltage(x, batts)), data))

def solve_part_1(data: str) -> int:
    return solve(data, 2)

def solve_part_2(data: str) -> int:
    return solve(data, 12)

if __name__ == "__main__":
    check_and_submit(3, "a", solve_part_1)
    check_and_submit(3, "b", solve_part_2)