from functools import reduce
from typing import NamedTuple
from enum import StrEnum

class Direction(StrEnum):
    LEFT = 'L'
    RIGHT = 'R'

class Rotation(NamedTuple):
    direction: Direction
    distance: int

zero_count = 0
dial_size = 100

def rotate(position: int, rotation: Rotation):
    global zero_count, dial_size

    if rotation.direction == Direction.LEFT:
        position = (position - rotation.distance) % dial_size
    elif rotation.direction == Direction.RIGHT:
        position = position + rotation.distance

    if position == 0:
        zero_count += 1
    return position

with open("./inputs/Day1.txt") as f:
    lines = f.readlines()

lines = [x.strip() for x in lines]
lines = [x for x in lines if len(x) > 0]

lines = map(lambda x: Rotation(Direction(x[0]), int(x[1:])), lines)

reduce(rotate, lines, 50)
print(zero_count)