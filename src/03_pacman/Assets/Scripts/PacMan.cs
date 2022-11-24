using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PacMan : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] public Grid grid;
    [SerializeField] private Tilemap walls;
    private int[] direction = { 1, 0 };
    private Rigidbody2D rb;

    public int score = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        int horizontal = Math.Sign(Input.GetAxis("Horizontal"));
        int vertical = Math.Sign(Input.GetAxis("Vertical"));
        Vector3Int position = GetPosition();
        Debug.Log("" + horizontal + "  " + walls.GetTile(new Vector3Int(position.x + horizontal, position.y)) + "   " + position);
        RaycastHit hitInfo;
        Debug.Log(grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        //Debug.Log(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity));
        //Debug.Log(hitInfo.point);
        if (horizontal != 0 && walls.GetTile(new Vector3Int(position.x + horizontal, position.y)) == null) {
            direction = new int[] { horizontal, 0 };
        } else if (vertical != 0 && walls.GetTile(new Vector3Int(position.x, position.y + vertical)) == null) {
            direction = new int[] { 0, vertical };
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(speed * direction[0], speed * direction[1]);

        if (direction[0] != 0) {
            transform.position = new Vector3(transform.position.x, grid.CellToWorld(GetPosition()).y + grid.cellSize.y / 2);
            transform.rotation = Quaternion.Euler(0, 0, (direction[0] - 1) * 90);
        } else {
            transform.position = new Vector3(grid.CellToWorld(GetPosition()).x + grid.cellSize.x / 2, transform.position.y);
            transform.rotation = Quaternion.Euler(0, 0, direction[1] * 90);
        }
    }

    public void AddScore(int score)
    {
        this.score += score;
    }

    public Vector3Int GetPosition()
    {
        //Debug.Log(grid.WorldToCell(transform.position));
        return grid.WorldToCell(transform.position);
    }
}
