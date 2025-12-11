from itertools import combinations
from functools import cache
from typing import List
from lib import check_and_submit, Point, Pair

@cache
def area(p: Pair) -> int:
    l = abs(p.b.x - p.a.x) + 1
    w = abs(p.b.y - p.a.y) + 1
    return l * w

@cache
def lrtb(p: Pair):
    return min([p.a.x, p.b.x]), max([p.a.x, p.b.x]), max([p.a.y, p.b.y]), min([p.a.y, p.b.y])

@cache
def overlaps(p: Pair, q: Pair) -> bool:
    """
    https://stackoverflow.com/questions/306316/determine-if-two-rectangles-overlap-each-other#306332
    https://silentmatt.com/rectangle-intersection/
    :param p:
    :param q:
    :return:
    """
    bp = lrtb(p)
    bq = lrtb(q)
    return not (
        bp[0] >= bq[1] or
        bp[1] <= bq[0] or
        bp[2] <= bq[3] or
        bp[3] >= bq[2]
    )

def get_pairs(points: list[Point]) -> List[Pair]:
    return [Pair(*x) for x in combinations(points, 2)]

def get_points(data: str) -> list[Point]:
    return [Point.from_str(x) for x in data.splitlines()]

def solve_part_1(data: str) -> int:
    pairs = get_pairs(get_points(data))
    most_areaness = max(pairs, key=area)
    return area(most_areaness)

def solve_part_2(data: str) -> int:
    points = get_points(data)
    pairs = get_pairs(points)
    pairs.sort(key=area, reverse=True)
    edges = [Pair(*x) for x in zip([points[-1]] + points, points)]

    # check if any edges overlap with rectangles. If not any overlap, it's either completely inside or completely
    # outside our polygon. However, no pair will be completely outside the polygon because the pairs are composed of the same
    # points as the edges
    winner = next(x for x in pairs if not any(overlaps(x, y) for y in edges))
    return area(winner)


if __name__ == "__main__":
    check_and_submit(9, "a", solve_part_1)
    check_and_submit(9, "b", solve_part_2)