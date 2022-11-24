using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private PacMan pacman;
    [SerializeField] private Grid grid;

    private List<Vector3Int> path = new List<Vector3Int>();
    private int[] direction = { 0, 0 };
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (path.Count == 0) {
            path = Pathfinding.instance.pathfind(GetPosition(), pacman.GetPosition());
        }
        FollowPath();
    }

    private void FollowPath()
    {
        Vector3Int pos = GetPosition();
        // Nothing to follow
        if (path.Count == 0) {
            return;
        }
        // We reached a destination (more or less)
        if (pos.x == path[0].x && pos.y == path[0].y) {
            path.RemoveAt(0);
            Vector2 newPos = grid.CellToWorld(pos);
            newPos.Set(newPos.x + grid.cellSize.x / 2, newPos.y + grid.cellSize.x / 2);
            transform.position = newPos;
            // Nothing to follow anymore :)
            if (path.Count == 0) {
                return;
            }
        }
        if (pos.x < path[0].x) {
            direction = new int[] { 1, 0 };
        } else if (pos.x > path[0].x) {
            direction = new int[] { -1, 0 };
        } else if (pos.y < path[0].y) {
            direction = new int[] { 0, 1 };
        } else if (pos.y > path[0].y) {
            direction = new int[] { 0, -1 };
        }
        rb.velocity = new Vector2(speed * direction[0], speed * direction[1]);
    }
    
    public Vector3Int GetPosition()
    {
        return grid.WorldToCell(transform.position);
    }
}
