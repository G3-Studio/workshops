import random

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
    [1, 0, 0, 1, 0, 0, 0, 0, 0, 0],
    [1, 0, 1, 1, 0, 0, 1, 1, 0, 0],
    [1, 0, 1, 0, 0, 0, 0, 1, 1, 1],
    [1, 0, 1, 1, 1, 1, 1, 1, 0, 1],
    [1, 0, 0, 0, 0, 1, 0, 0, 0, 1],
    [1, 1, 0, 0, 0, 1, 0, 0, 0, 1],
    [0, 1, 0, 0, 0, 1, 1, 1, 0, 1],
    [2, 1, 1, 1, 1, 1, 0, 1, 1, 1],
]

#node_pattern = [[int(random.random() > 0.3) for j in range(20)] for i in range(20)]
#node_pattern[0][-1] = 3
#node_pattern[-1][0] = 2


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
font = None


def display_step(nodes, opened_nodes, closed_nodes, current):
    global screen, font
    if screen is None:
        pygame.init()
        screen = pygame.display.set_mode((500, 500))
        font = pygame.font.SysFont("Arial", 16)
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
        node_surf.fill(color)
        if node in opened_nodes or node in closed_nodes:
            node_surf.blit(font.render(str(node.cost), True, (0, 0, 0)), (5, 5))
            distance_to_dest_surf = font.render(str(node.distance_to_destination), True, (0, 0, 0))
            node_surf.blit(distance_to_dest_surf, (45 - distance_to_dest_surf.get_width(), 5))
            heuristic_surf = font.render(str(node.heuristic_cost), True, (0, 0, 0))
            node_surf.blit(heuristic_surf, ((50 - heuristic_surf.get_width()) / 2, 25))
        if node.comes_from.x != -1:
            if node.pos.x > node.comes_from.x:
                pygame.draw.polygon(node_surf, (0, 0, 0), [(5, 20), (5, 30), (10, 25)])
            elif node.pos.x < node.comes_from.x:
                pygame.draw.polygon(node_surf, (0, 0, 0), [(45, 20), (45, 30), (40, 25)])
            elif node.pos.y > node.comes_from.y:
                pygame.draw.polygon(node_surf, (0, 0, 0), [(20, 5), (30, 5), (25, 10)])
            else:
                pygame.draw.polygon(node_surf, (0, 0, 0), [(20, 45), (30, 45), (25, 40)])
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
        node_surf.fill(color)
        if node in path:
            if node.comes_from.x != -1:
                if node.pos.x > node.comes_from.x:
                    pygame.draw.polygon(node_surf, (0, 0, 0), [(5, 20), (5, 30), (10, 25)])
                elif node.pos.x < node.comes_from.x:
                    pygame.draw.polygon(node_surf, (0, 0, 0), [(45, 20), (45, 30), (40, 25)])
                elif node.pos.y > node.comes_from.y:
                    pygame.draw.polygon(node_surf, (0, 0, 0), [(20, 5), (30, 5), (25, 10)])
                else:
                    pygame.draw.polygon(node_surf, (0, 0, 0), [(20, 45), (30, 45), (25, 40)])
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


