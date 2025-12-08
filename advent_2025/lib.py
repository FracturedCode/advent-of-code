from collections.abc import Callable
from typing import Literal
from stopwatch import Stopwatch
from aocd.models import Puzzle
import aocd, pprint

year = 2025

def check_and_submit(day: int, a_or_b: Literal["a", "b"], solve: Callable[[str], int]):
    check_failed = False
    puzzle = Puzzle(year=year, day=day)
    for example in puzzle.examples:
        check_answer = str(solve(example.input_data))
        expected_answer = example.answer_a if a_or_b == "a" else example.answer_b
        if check_answer != expected_answer:
            check_failed = True
            print(f"\n\nCheck failed. Expected: {expected_answer} Actual: {check_answer}")
            print(f"Example used: {pprint.pformat(example)} \n\n")

    sw = Stopwatch()
    answer = str(solve(puzzle.input_data))
    sw.stop()
    print(f"\nDay{day}Part{a_or_b.upper()} in {sw}\nAnswer: {answer}")

    if puzzle.answered(a_or_b):
        print(f"You have already submitted a correct answer to this part.")
        expected_answer = puzzle.answer_a if a_or_b == "a" else puzzle.answer_b
        if answer != expected_answer:
            print(f"Your current solution is incorrect! Expected: {expected_answer} Actual: {answer}")
    if not puzzle.answered(a_or_b) and not check_failed:
        aocd.submit(answer, day=day, year=year, part=a_or_b)

    print()