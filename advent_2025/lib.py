from collections.abc import Callable
from functools import cache
from typing import Literal, NamedTuple, Any, Protocol, runtime_checkable
from stopwatch import Stopwatch
from aocd.models import Puzzle
import aocd, pprint

year = 2025

custom_example_invocations = {
    (8, "a"): lambda solve, data: solve(data, 10)
}

def check_and_submit(day: int, a_or_b: Literal["a", "b"], solve):
    check_failed = False
    puzzle = Puzzle(year=year, day=day)
    for example in puzzle.examples:
        ci = custom_example_invocations.get((day, a_or_b))
        check_answer = ci(solve, example.input_data) if ci else solve(example.input_data)
        check_answer = str(check_answer)
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

class Range(NamedTuple):
    start: int
    stop_inclusive: int

    def __iter__(self):
        return iter(range(self.start, self.stop_inclusive+1))

class Point(NamedTuple):
    x: int = 0
    y: int = 0
    z: int = 0

    @staticmethod
    def from_str(s: str) -> Point:
        return Point(*[int(x) for x in s.split(",")])

@cache
def distance(a: Point, b: Point) -> float:
    return ((b.x - a.x) ** 2 + (b.y - a.y) ** 2 + (b.z - a.z) ** 2) ** .5

class Pair(NamedTuple):
    a: Point
    b: Point

    def distance(self):
        return distance(*self)