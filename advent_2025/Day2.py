from typing import NamedTuple
from py_linq import Enumerable
from lib import check_and_solve

class Range(NamedTuple):
    start: int
    stop_inclusive: int

def num_repeats_twice(num: str) -> bool:
    midpoint = len(num) // 2
    return num[:midpoint] == num[midpoint:]

def num_repeats(num: str) -> int:
    ln = len(num)
    for i in range(1, ln // 2 + 1):
        if ln % i == 0:
            repeat = num[:i]
            if repeat * (ln // i) == num:
                return int(num)
    return 0

def parse_and_get_all_nums(data: str) -> Enumerable:
    data = Enumerable(data.strip().split(','))
    data = data.select(lambda x: x.split('-')).select(lambda x: Range(int(x[0]), int(x[1]))).to_list()

    numbers = sum(map(lambda x: x.stop_inclusive - x.start + 1, data))
    print(f'Input length: {len(data)}\nTotal numbers: {numbers}')
    return Enumerable(data).select_many(lambda x: list(range(x.start, x.stop_inclusive + 1))).select(str)

def solve_part_1(data: str) -> int:
    filtered = parse_and_get_all_nums(data).where(lambda x: not len(x) % 2 and num_repeats_twice(x))
    return filtered.sum(int)

def solve_part_2(data: str) -> int:
    return parse_and_get_all_nums(data).sum(num_repeats)

example_data = "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124"

check_and_solve(2, example_data, 1227775554, solve_part_1)
check_and_solve(2, example_data, 4174379265, solve_part_2)