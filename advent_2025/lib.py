from collections.abc import Callable

from aocd import get_data

def check_and_solve(day: int, year: int, example_data: str, example_answer: int, func: Callable[[str], int]):
    check_answer = func(example_data)
    if check_answer != example_answer:
        print(f"Check failed with output: {check_answer}")

    solve_answer = func(get_data(day=day, year=year))
    print(f"Solution: {solve_answer}")