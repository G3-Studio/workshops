import pygame
from Node import Node


class List2D(list):
    def __init__(self, height, width):
        super().__init__()
        for i in range(height):
            self.append([None for i in range(width)])

    def __iter__(self):
        for row in super().__iter__():
            yield from row

    def __getitem__(self, item):
        return super().__getitem__(item[0])[item[1]]

    def __setitem__(self, key, value):
        super().__getitem__(key[0])[key[1]] = value


class Vector2(tuple):

    def __getattribute__(self, item):
        if item == "x":
            return self[0]
        if item == "y":
            return self[1]
        return super().__getattribute__(item)

    def __add__(self, other):
        if isinstance(other, tuple):
            return Vector2((self[0] + other[0], self[1] + other[1]))
        else:
            return Vector2((self[0] + other, self[1] + other))


node_pattern = [
    [0, 0, 0, 0, 0, 0, 0, 0, 1, 3],
    [1, 1, 1, 1, 1, 1, 1, 1, 1, 0],
    [1, 0, 0, 0, 0, 0, 1, 0, 0, 0],
    [1, 0, 0, 0, 1, 1, 1, 1, 0, 0],
    [1, 0, 0, 0, 1, 0, 0, 1, 1, 1],
    [1, 0, 0, 1, 1, 1, 1, 1, 0, 1],
    [1, 0, 0, 1, 0, 0, 0, 0, 0, 1],
    [1, 1, 0, 1, 1, 1, 0, 0, 0, 1],
    [0, 1, 0, 0, 0, 1, 1, 1, 0, 1],
    [2, 1, 1, 1, 1, 1, 0, 0, 1, 1],
]


nodes = List2D(10, 10)
start = Vector2((0, 0))
end = Vector2((0, 0))


def load_nodes():
    global start, end
    for y, row in enumerate(node_pattern):
        for x, nb in enumerate(row):
            if nb == 2:
                start = Vector2((x, y))
            elif nb == 3:
                end = Vector2((x, y))
            nodes[x, y] = Node(Vector2((x, y)), nb == 0)
    return nodes


def get_start():
    return start


def get_end():
    return end


screen = None


def display_step(nodes, opened_nodes, closed_nodes, current):
    global screen
    if screen is None:
        pygame.init()
        screen = pygame.display.set_mode((500, 500))
    for node in nodes:
        node_surf = pygame.Surface((50, 50))
        color = (255, 0, 0)
        if node.is_wall:
            color = (0, 0, 0)
        if node in opened_nodes:
            color = (0, 255, 0)
        if node in closed_nodes:
            color = (0, 0, 255)
        if node is current:
            color = (255, 255, 0)
        pygame.draw.rect(node_surf, color, pygame.Rect(0, 0, 50, 50))
        screen.blit(node_surf, (node.pos.x * 50, node.pos.y * 50))
    pygame.display.flip()
    while True:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.display.quit()
                exit(0)
            if event.type == pygame.KEYDOWN:
                return


def display_and_quit(nodes, path):
    global screen
    if screen is None:
        pygame.init()
        screen = pygame.display.set_mode((500, 500))
    for node in nodes:
        node_surf = pygame.Surface((50, 50))
        color = (255, 0, 0)
        if node.is_wall:
            color = (0, 0, 0)
        if node in path:
            color = (0, 255, 255)
        pygame.draw.rect(node_surf, color, pygame.Rect(0, 0, 50, 50))
        screen.blit(node_surf, (node.pos.x * 50, node.pos.y * 50))
    pygame.display.flip()
    while True:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.display.quit()
                exit(0)
            if event.type == pygame.KEYDOWN:
                exit(0)


class NodePriorityQueue(list):
    def pop(self):
        self.sort(key=lambda node: node.heuristic_cost)
        return super().pop(0)


