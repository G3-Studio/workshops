using System;
using UnityEngine;

public class PacMan : MonoBehaviour
{

    [SerializeField] private float speed = 5f;
    public int[] direction = { 1, 0 };
    public Rigidbody2D rb;

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
        rb.velocity = new Vector3(speed * direction[0], speed * direction[1], 0);

        if (direction[0] != 0) {
            transform.rotation = Quaternion.Euler(0, 0, (direction[0] - 1) * 90);
        } else {
            transform.rotation = Quaternion.Euler(0, 0, direction[1] * 90);
        }
    }

    public void addScore(int score)
    {
        this.score += score;
    }
}
