from collections.abc import Callable
from stopwatch import Stopwatch
from aocd.models import Puzzle
import pprint

def check_and_solve(day: int, a_or_b: str, func: Callable[[str], int]):
    puzzle = Puzzle(year=2025, day=day)
    for example in puzzle.examples:
        check_answer = str(func(example.input_data))
        expected_answer = example.answer_a if a_or_b == "a" else example.answer_b
        if check_answer != expected_answer:
            print(f"\n\nCheck failed. Expected: {expected_answer} Actual: {check_answer}")
            print(f"Example used: {pprint.pformat(example)} \n\n")

    sw = Stopwatch()
    solve_answer = func(puzzle.input_data)
    sw.stop()
    print(f"\nSolution: {solve_answer} in {sw}\n")