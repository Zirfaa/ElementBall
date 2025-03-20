using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementWith : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed = 8f;
    private float jumpForce = 12f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        if(Input.GetKeyDown(KeyCode.Space)) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * 10);
        }
    }
}
