from aocd import get_data

from functools import reduce
from typing import NamedTuple
from enum import StrEnum
import math

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
    global dial_size

    pos = state.dial_position + rotation.distance * (-1 if rotation.direction == Direction.LEFT else 1)

    zero_stopped = pos % dial_size == 0

    rotation_zero_seen_count = abs(pos // dial_size - state.dial_position // dial_size)
    rotation_zero_seen_count += rotation.direction == Direction.LEFT and zero_stopped
    # don't double count when we were already stopped on zero
    rotation_zero_seen_count -= rotation.direction == Direction.LEFT and state.dial_position % dial_size == 0

    state = PuzzleState(
        pos,
        state.zero_stop_count + zero_stopped,
        state.zero_seen_count + rotation_zero_seen_count)
    #print(state)
    return state

example_data = """
L68
L30
R48
L5
R60
L55
L1
L99
R14
L82
"""
data = get_data(day=1, year=2025).strip().splitlines()
#data = example_data.strip().splitlines()
data = map(lambda x: Rotation(Direction(x[0]), int(x[1:])), data)

puzzle_state = reduce(rotate, data, PuzzleState(50, 0, 0))
print(puzzle_state)