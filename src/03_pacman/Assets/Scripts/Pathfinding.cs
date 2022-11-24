using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding instance { get; private set; }
    
    [SerializeField] private Tilemap walls;
    
    private Pathfinding()
    {
        instance = this;
    }

    public List<Vector3Int> pathfind(Vector3Int start, Vector3Int end)
    {
        List<Node> openedNodes = new List<Node>();
        List<Node> closedNodes = new List<Node>();
        openedNodes.Add(new Node(start, end, walls.GetTile(start) != null));
        // While there are still Nodes to search for
        while (openedNodes.Count != 0) {
            // We sort it to make as if it was a PriorityQueue
            openedNodes.Sort((node1, node2) => node1.heuristicCost - node2.heuristicCost);
            Node current = openedNodes[0];
            openedNodes.RemoveAt(0);
            closedNodes.Add(current);
            // We arrived at the end, we just need to go back
            if (current.pos.x == end.x && current.pos.y == end.y) {
                List<Vector3Int> path = new List<Vector3Int>();
                path.Add(current.pos);
                Debug.Log("starting");
                while (current.pos.x != start.x || current.pos.y != start.y) {
                    current = current.comesFrom;
                    path.Add(current.pos);
                    Debug.Log("adding a new one !!" + current.pos + " qui est un mur ? " +
                              (walls.GetTile(current.pos) != null));
                }
                // Reverse the path to have the start node first
                path.Reverse();
                /*String content = "";
                foreach (var p in path) {
                    content += "  " + p.ToString();
                }
                Debug.Log(content);*/
                return path;
            }
            // Create the list of neighbours
            List<Vector3Int> neighbours = new List<Vector3Int>();
            neighbours.Add(new Vector3Int(current.pos.x, current.pos.y - 1));
            neighbours.Add(new Vector3Int(current.pos.x + 1, current.pos.y));
            neighbours.Add(new Vector3Int(current.pos.x, current.pos.y + 1));
            neighbours.Add(new Vector3Int(current.pos.x - 1, current.pos.y));
            foreach (Vector3Int neighbour in neighbours) {
                // Neighbour is not in closedNodes
                if (closedNodes.Exists(node => node.pos.x == neighbour.x && node.pos.y == neighbour.y)) {
                    continue;
                }
                // We get the neighbour nodes from the openedNodes list. If it doesn't exist, we create it
                Node neighbourNode = openedNodes.Find(node => node.pos.x == neighbour.x && node.pos.y == neighbour.y);
                bool isInOpenedNodes = true;
                if (neighbourNode == null) {
                    neighbourNode = new Node(neighbour, end, walls.GetTile(neighbour) != null);
                    isInOpenedNodes = false;
                }
                // The "moving forward" part of the algorithm
                if (!neighbourNode.isWall) {
                    if (!isInOpenedNodes) {
                        neighbourNode.cost = current.cost + 1;
                        neighbourNode.heuristicCost = neighbourNode.cost + neighbourNode.distanceToEnd;
                        neighbourNode.comesFrom = current;
                        openedNodes.Add(neighbourNode);
                    } else if (current.cost + 1 < neighbourNode.cost) {
                        neighbourNode.cost = current.cost + 1;
                        neighbourNode.heuristicCost = neighbourNode.cost + neighbourNode.distanceToEnd;
                        neighbourNode.comesFrom = current;
                    }
                }
            }
        }

        // We should never fall in this case, but we still return something, in case something goes horribly wrong
        // (that would be so chocking to have a code with bugs ^^')
        return new List<Vector3Int>();
    }
}

class Node
{

    public Vector3Int pos;
    private Vector3Int end;
    public bool isWall;
    public int cost;
    public int distanceToEnd;
    public int heuristicCost;
    public Node comesFrom;

    public Node(Vector3Int _pos, Vector3Int _end, bool _isWall)
    {
        pos = _pos;
        end = _end;
        isWall = _isWall;
        cost = 0;
        distanceToEnd = Math.Abs(pos.x - end.x) + Math.Abs(pos.y - end.y);
        heuristicCost = distanceToEnd;
    }
}