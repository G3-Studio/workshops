using System;
using UnityEngine;

public class PacMan : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] public Grid grid;
    private int[] direction = { 1, 0 };
    private Rigidbody2D rb;

    public int score = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0) {
            direction = new[] { Math.Sign(horizontal), 0 };
        } else if (vertical != 0) {
            direction = new[] { 0, Math.Sign(vertical)};
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(speed * direction[0], speed * direction[1]);

        if (direction[0] != 0) {
            transform.position = new Vector3(transform.position.x, grid.CellToWorld(GetPosition()).y + 0.5f);
            transform.rotation = Quaternion.Euler(0, 0, (direction[0] - 1) * 90);
        } else {
            transform.position = new Vector3(grid.CellToWorld(GetPosition()).x + 0.5f, transform.position.y);
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
