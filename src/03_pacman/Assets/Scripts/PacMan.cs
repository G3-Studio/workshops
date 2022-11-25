using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class PacMan : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap walls;
    
    public int[] direction { get; private set; } = { 1, 0 };
    private Rigidbody2D rb;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;

    public int score = 0;
    private Vector3Int previousPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        int horizontal = Math.Sign(Input.GetAxis("Horizontal"));
        int vertical = Math.Sign(Input.GetAxis("Vertical"));
        Vector3Int position = GetPosition();
        if (horizontal != 0 && walls.GetTile(new Vector3Int(position.x + horizontal, position.y)) == null) {
            direction = new int[] { horizontal, 0 };
        } else if (vertical != 0 && walls.GetTile(new Vector3Int(position.x, position.y + vertical)) == null) {
            direction = new int[] { 0, vertical };
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(speed * direction[0], speed * direction[1], 0);

        if (direction[0] != 0) {
            transform.position = new Vector3(transform.position.x, grid.CellToWorld(GetPosition()).y + grid.cellSize.y / 2);
            transform.rotation = Quaternion.Euler(0, 0, (direction[0] - 1) * 90);
        } else {
            transform.position = new Vector3(grid.CellToWorld(GetPosition()).x + grid.cellSize.x / 2, transform.position.y);
            transform.rotation = Quaternion.Euler(0, 0, direction[1] * 90);
        }

        Vector3Int currentPosition = this.GetPosition();
        if (previousPosition == null || previousPosition != currentPosition) {
            enemy1.GetComponent<NavMeshAgent>().destination = transform.position;
            enemy2.GetComponent<NavMeshAgent>().destination = transform.position;
            enemy3.GetComponent<NavMeshAgent>().destination = transform.position;
            enemy4.GetComponent<NavMeshAgent>().destination = transform.position;
            this.previousPosition = currentPosition;
        }
    }

    private Vector3Int GetPosition() {
        return grid.WorldToCell(transform.position);
    }

    public void addScore(int score)
    {
        this.score += score;
    }
}
