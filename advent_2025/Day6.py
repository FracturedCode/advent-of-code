from functools import reduce
from typing import Literal, cast, List
from py_linq import Enumerable
from lib import check_and_submit, Range
import re
from dataclasses import dataclass

@dataclass
class Problem:
    operation: Literal["*", "+"]
    range: Range
    operands: list[int]

    @staticmethod
    def from_range(r: Range, part: Literal["a", "b"], data: list[str]) -> Problem:
        operation = data[-1][r.start]
        operand_rows = data[:-1]
        if part == "a":
            operands = list(map(lambda x: int(x[r.start:r.stop_inclusive+1]), operand_rows))
        else:
            operands = Enumerable(r).select(lambda x: int(reduce(lambda y, z: y + z[x], operand_rows, ""))).to_list()
        return Problem(cast(Literal["*", "+"], operation), r, operands)

    def solve(self) -> int:
        fn = (lambda x, y: x * y) if self.operation == "*" else (lambda x, y: x + y)
        return reduce(fn, self.operands)

def solve(data: str, part: Literal["a", "b"]) -> int:
    data = [x for x in data.splitlines() if x != ""]
    operation_row = data[-1]

    # Derive bounds of each problem via operator positions
    cols = list(map(lambda x: x.end() - 1, re.finditer(r"( |^)([*+])", operation_row)))
    cols = zip(cols, cols[1:] + [len(operation_row) + 1])
    cols = map(lambda x: (x[0], x[1] - 2), cols)
    cols = list(map(lambda x: Range(*x), cols))

    # Finish parsing problems and calculate solution
    cols = map(lambda x: Problem.from_range(x, part, data), cols)
    return sum(map(lambda x: x.solve(), cols))

def solve_part_1(data: str) -> int:
    return solve(data, "a")

def solve_part_2(data: str) -> int:
    return solve(data, "b")

if __name__ == "__main__":
    check_and_submit(6, "a", solve_part_1)
    check_and_submit(6, "b", solve_part_2)