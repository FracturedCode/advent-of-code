from collections.abc import Callable

from aocd import get_data
from stopwatch import Stopwatch


def check_and_solve(day: int, example_data: str, example_answer: int, func: Callable[[str], int]):
    check_answer = func(example_data)
    if check_answer != example_answer:
        print(f"Check failed with output: {check_answer}")

    sw = Stopwatch()
    solve_answer = func(get_data(day=day, year=2025))
    sw.stop()
    print(f"Solution: {solve_answer} in {sw}")