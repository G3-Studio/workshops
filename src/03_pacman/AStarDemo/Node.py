import utils


class Node:

    def __init__(self, pos, is_wall):
        self.pos = pos
        self.cost = 0
        self.distance_to_destination = 0
        self.heuristic_cost = 0
        self.comes_from = utils.Vector2((-1, -1))
        self.is_wall = is_wall

    def __repr__(self):
        return f"<Node is_wall={self.is_wall}, pos={self.pos}>"
