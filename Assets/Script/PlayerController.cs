using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.WSA;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float wallSlideSpeed = 10f;
    [SerializeField] private float wallStickGravity = 0.5f;
    [SerializeField] private float wallSpeed = 5f;
    public Transform wallCheck;
    public Transform groundCheck;
    public LayerMask wallLayer;
    public LayerMask groundLayer;
    public Color buffColor;
    public GameObject bounceBall;
    public GameObject normal;
    public bool isBuff;
    private float movement;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isOnWall;
    private bool isWallSticking;
    private float verticalInput;
    private float originalJumpForce;
    private SpriteRenderer sr;
    private Color originalColor;
    private GameObject currentPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        originalJumpForce = jumpForce;
        currentPlayer = gameObject;
    }
    
    private void Update()
        {
        isOnWall = Physics2D.OverlapCircle(wallCheck.position, 0.5f, wallLayer);
        isWallSticking = isOnWall;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer |wallLayer);
        if (isBuff)
        {
            if (isWallSticking)
            {
                if (rb.velocity.x == 0)
                {
                    rb.velocity = new Vector2(rb.velocity.y, -wallSlideSpeed);
                }
                else if(isOnWall)
                {
                    rb.velocity = new Vector2(rb.velocity.x, verticalInput* wallSpeed);
                }

            }
            else
            {
                rb.gravityScale = 1;
            }
        }
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
        if(other.CompareTag("Bounce"))
        {
            other.gameObject.SetActive(false);
            StartCoroutine(ApplyBounce(10f));
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
        isBuff = true;
        buffColor = Color.cyan;
        sr.color = buffColor;
        yield return new WaitForSeconds(duration);
        sr.color = originalColor;
        isBuff = false;

    }
    IEnumerator ApplyBounce(float duration)
    {
        
        Vector3 position = currentPlayer.transform.position;
        Quaternion rotation = currentPlayer.transform.rotation;
        Rigidbody2D oldRb = currentPlayer.GetComponent<Rigidbody2D>();
        GameObject newPlayer = Instantiate(bounceBall, position, rotation);
        newPlayer.GetComponent<Rigidbody2D>().velocity = oldRb.velocity;
        Destroy(currentPlayer);
        currentPlayer = newPlayer;
        yield return new WaitForSeconds(duration);
        position = newPlayer.transform.position;
        rotation = newPlayer.transform.rotation;
        oldRb = currentPlayer.GetComponent<Rigidbody2D>();
        GameObject originalPlayer = Instantiate(normal, position, rotation);
        originalColor = originalPlayer.GetComponent<Color>();
        Destroy(currentPlayer);
        currentPlayer = originalPlayer;

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
