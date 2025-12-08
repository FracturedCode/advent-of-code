from functools import reduce
from typing import NamedTuple
from enum import StrEnum
from lib import check_and_submit

class Direction(StrEnum):
    LEFT = 'L'
    RIGHT = 'R'

class Rotation(NamedTuple):
    direction: Direction
    distance: int

class PuzzleState(NamedTuple):
    dial_position: int
    zero_stop_count: int
    zero_seen_count: int

dial_size = 100

def rotate(state: PuzzleState, rotation: Rotation):
    pos = state.dial_position + rotation.distance * (-1 if rotation.direction == Direction.LEFT else 1)

    zero_stopped = pos % dial_size == 0

    rotation_zero_seen_count = abs(pos // dial_size - state.dial_position // dial_size)
    rotation_zero_seen_count += rotation.direction == Direction.LEFT and zero_stopped
    # don't double count when we were previously on zero
    rotation_zero_seen_count -= rotation.direction == Direction.LEFT and state.dial_position % dial_size == 0

    state = PuzzleState(
        pos,
        state.zero_stop_count + zero_stopped,
        state.zero_seen_count + rotation_zero_seen_count)
    #print(state)
    return state

def solve(data: str) -> PuzzleState:
    data = data.strip().splitlines()
    data = map(lambda x: Rotation(Direction(x[0]), int(x[1:])), data)
    puzzle_state = reduce(rotate, data, PuzzleState(50, 0, 0))
    print(puzzle_state)
    return puzzle_state

if __name__ == "__main__":
    check_and_submit(1, "a", lambda x: solve(x).zero_stop_count)
    check_and_submit(1, "b", lambda x: solve(x).zero_seen_count)