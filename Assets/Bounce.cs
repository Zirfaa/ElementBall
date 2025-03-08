using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    private float movement;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        movement = Input.GetAxis("Horizontal");
    }
    private void FixedUpdate()
    {
        rb.velocity=new Vector2(movement*moveSpeed, rb.velocity.y);
    }
}
