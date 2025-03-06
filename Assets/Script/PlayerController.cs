using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float wallSlideSpeed = 10f;
    [SerializeField] private float wallStickGravity = 0.5f;
    [SerializeField] private float wallSpeed = 3f;
    public Transform wallCheck;
    public Transform groundCheck;
    public LayerMask wallLayer;
    public LayerMask groundLayer;
    public Color buffColor;
    private float movement;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isOnWall;
    private bool isWallSticking;
    private float verticalInput;
    private float originalJumpForce;
    private SpriteRenderer sr;
    private Color originalColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        originalJumpForce = jumpForce;
        
    }
    
    private void Update()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer |wallLayer);
            isOnWall = Physics2D.OverlapCircle(wallCheck.position, 0.6f, wallLayer);
            isWallSticking = isOnWall && Input.GetMouseButton(0);
        if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        movement = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player menyentuh: " + other.gameObject.name);
        if (other.CompareTag("BuffJump")) 
        { 
            StartCoroutine(ApplyJumpBoost(5f, 8f));
            other.gameObject.SetActive(false);
            StartCoroutine(Respawn(other.gameObject, 10f));
        }
        if (other.CompareTag("StickBuff"))
        {
            StartCoroutine(ApplyStiky(5f));
            other.gameObject.SetActive(false);
            StartCoroutine(Respawn(other.gameObject, 10f));
        }
    }
    IEnumerator ApplyJumpBoost(float duration,float boost)
    {
        jumpForce = boost;
        buffColor = Color.green;
        sr.color = buffColor;
        yield return new WaitForSeconds(duration);
        sr.color = originalColor;
        jumpForce = originalJumpForce;
    }
    IEnumerator ApplyStiky(float duration)
    {
        if (isWallSticking)
        {
            if (rb.velocity.y == 0)
            {
                rb.velocity = new Vector2(rb.velocity.y, -wallSlideSpeed);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, verticalInput * wallSpeed);
            }
            Debug.Log("Velocity X: " + rb.velocity.x);
            yield return new WaitForSeconds(duration);
        }
        else
        {
            rb.gravityScale = 1;
        }

    }
    IEnumerator Respawn(GameObject buff,float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);
        buff.SetActive(true);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2 (movement *moveSpeed,rb.velocity.y);
        
    }

}
