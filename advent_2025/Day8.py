from functools import cache
from typing import NamedTuple, Iterable
from py_linq import Enumerable
from lib import check_and_submit
from itertools import combinations

class Coord(NamedTuple):
    x: int
    y: int
    z: int

    @staticmethod
    def from_str(s: str) -> Coord:
        return Coord(*[int(x) for x in s.split(",")])

@cache
def distance(a: Coord, b: Coord) -> float:
    return ((b.x - a.x) ** 2 + (b.y - a.y) ** 2 + (b.z - a.z) ** 2) ** .5

class Pair(NamedTuple):
    a: Coord
    b: Coord

    def distance(self):
        return distance(*self)

class Graph:
    def __init__(self, nodes: set[Coord]):
        self.nodes = {x: x for x in nodes}

    def find(self, x: Coord) -> Coord:
        return x if self.nodes[x] == x else self.find(self.nodes[x])

    def union(self, a: Coord, b: Coord):
        self.nodes[self.find(b)] = self.find(a)

    def is_connected(self):
        ni = iter(self.nodes)
        parent = self.find(self.nodes[next(ni)])
        return all(map(lambda x: self.find(self.nodes[x]) == parent, ni))

def get_sorted_pairs(coords: Iterable[Coord]) -> list[Pair]:
    pairs = [Pair(a, b) for a, b in combinations(coords, 2)]
    pairs.sort(key=lambda x: x.distance())
    return pairs

def solve_part_1(data: str, nodes: int = 1000) -> int:
    coords = map(Coord.from_str, data.splitlines())
    pairs = get_sorted_pairs(coords)[:nodes]
    coords = {x for pair in pairs for x in (pair.a, pair.b)}
    graph = Graph(coords)

    for pair in pairs:
        graph.union(pair.a, pair.b)

    cluster_sizes = Enumerable(coords).select(graph.find).group_by().select(lambda x: x.count())
    return cluster_sizes.order_by_descending(lambda x: x).take(3).aggregate(lambda x, y: x * y)

def solve_part_2(data: str) -> int:
    coords = set(map(Coord.from_str, data.splitlines()))
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