from functools import cache
from typing import NamedTuple, Iterable
from py_linq import Enumerable
from lib import check_and_submit, Point, Pair
from itertools import combinations

class Graph:
    def __init__(self, nodes: set[Point]):
        self.nodes = {x: x for x in nodes}

    def find(self, x: Point) -> Point:
        return x if self.nodes[x] == x else self.find(self.nodes[x])

    def union(self, a: Point, b: Point):
        self.nodes[self.find(b)] = self.find(a)

    def is_connected(self):
        ni = iter(self.nodes)
        parent = self.find(self.nodes[next(ni)])
        return all(map(lambda x: self.find(self.nodes[x]) == parent, ni))

def get_sorted_pairs(coords: Iterable[Point]) -> list[Pair]:
    pairs = [Pair(a, b) for a, b in combinations(coords, 2)]
    pairs.sort(key=lambda x: x.distance())
    return pairs

def solve_part_1(data: str, nodes: int = 1000) -> int:
    coords = map(Point.from_str, data.splitlines())
    pairs = get_sorted_pairs(coords)[:nodes]
    coords = {x for pair in pairs for x in (pair.a, pair.b)}
    graph = Graph(coords)

    for pair in pairs:
        graph.union(pair.a, pair.b)

    cluster_sizes = Enumerable(coords).select(graph.find).group_by().select(lambda x: x.count())
    return cluster_sizes.order_by_descending(lambda x: x).take(3).aggregate(lambda x, y: x * y)

def solve_part_2(data: str) -> int:
    coords = set(map(Point.from_str, data.splitlines()))
    pairs = get_sorted_pairs(coords)
    graph = Graph(coords)

    for i in range(len(pairs)):
        pair = pairs[i]
        graph.union(*pair)
        if graph.is_connected():
            break

    return pair.a.x * pair.b.x


if __name__ == "__main__":
    check_and_submit(8, "a", solve_part_1)
    check_and_submit(8, "b", solve_part_2)