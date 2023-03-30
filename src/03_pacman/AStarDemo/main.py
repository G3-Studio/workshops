import utils

nodes = utils.load_nodes()
start = utils.get_start()
end = utils.get_end()

for node in nodes:
    node.distance_to_destination = abs(end.x - node.pos.x) + abs(end.y - node.pos.y)
    node.heuristic_cost = node.distance_to_destination

opened_nodes = utils.NodePriorityQueue()
closed_nodes = []

opened_nodes.append(nodes[start])

utils.display_step(nodes, opened_nodes, closed_nodes, None)

while len(opened_nodes) != 0:
    current = opened_nodes.pop()
    closed_nodes.append(current)
    if current.pos.x == end.x and current.pos.y == end.y:
        path = [current]
        while current.pos != start:
            current = nodes[current.comes_from]
            path.append(current)
        utils.display_and_quit(nodes, path)
    neighbours = []
    if current.pos.y != 0:
        neighbours.append(nodes[current.pos + (0, -1)])
    if current.pos.x < 9:
        neighbours.append(nodes[current.pos + (1, 0)])
    if current.pos.y < 9:
        neighbours.append(nodes[current.pos + (0, 1)])
    if current.pos.x != 0:
        neighbours.append(nodes[current.pos + (-1, 0)])
    for neighbour in neighbours:
        if not neighbour.is_wall and neighbour not in closed_nodes:
            if neighbour not in opened_nodes:
                neighbour.cost = current.cost + 1
                neighbour.comes_from = current.pos
                neighbour.heuristic_cost = neighbour.cost + neighbour.distance_to_destination
                opened_nodes.append(neighbour)
            elif current.cost + 1 < neighbour.cost:
                neighbour.cost = current.cost + 1
                neighbour.distance_to_destination = abs(end.x - neighbour.pos.x) + abs(end.y - neighbour.pos.y)
                neighbour.comes_from = current.pos
                neighbour.heuristic_cost = neighbour.cost + neighbour.distance_to_destination
    utils.display_step(nodes, opened_nodes, closed_nodes, current)

