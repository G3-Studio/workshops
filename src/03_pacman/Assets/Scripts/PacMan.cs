using System;
using UnityEngine;

public class PacMan : MonoBehaviour
{

    [SerializeField] private float speed;
    public int[] direction { get; private set; } = { 1, 0 };
    public Rigidbody2D rb;


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
        Vector3 position = transform.position;
        if (direction[0] != 0) {
            transform.position = new Vector3(position.x, (float) Math.Round(position.y + 0.5) - 0.5f);
            transform.rotation = Quaternion.Euler(0, 0, (direction[0] - 1) * 90);
        } else {
            transform.position = new Vector3((float) Math.Round(position.x + 0.5) - 0.5f, position.y);
            transform.rotation = Quaternion.Euler(0, 0, direction[1] * 90);
        }
    }
}
