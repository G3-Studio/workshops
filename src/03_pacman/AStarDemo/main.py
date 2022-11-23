import Utils

nodes = Utils.load_nodes()
start = Utils.get_start()
end = Utils.get_end()

for node in nodes:
    node.distance_to_end = abs(end.x - node.pos.x) + abs(end.y - node.pos.y)

opened_nodes = Utils.NodePriorityQueue()
closed_nodes = []

opened_nodes.append(nodes[start])

Utils.display_step(nodes, opened_nodes, closed_nodes, None)

while len(opened_nodes) != 0:
    current = opened_nodes.pop()
    closed_nodes.append(current)
    if current.pos.x == end.x and current.pos.y == end.y:
        path = [current]
        while current.comes_from != start:
            current = nodes[current.comes_from]
            path.append(current)
        Utils.display_and_quit(nodes, path)
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
                opened_nodes.append(neighbour)
            elif current.cost + 1 < neighbour.cost:
                neighbour.cost = current.cost + 1
                neighbour.distance_to_end = abs(end.x - neighbour.pos.x) + abs(end.y - neighbour.pos.y)
                neighbour.comes_from = current.pos
    Utils.display_step(nodes, opened_nodes, closed_nodes, current)

