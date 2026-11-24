using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float movespeed;
    public float jumpForce;

    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    private float moveHorizontal;

    private float jumpTimer;
    public float jumpTime;
    private bool isJumping;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movespeed = 5f;
        jumpForce = 5f;
        isGrounded = false;
    }

    private void FixedUpdate()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveHorizontal * movespeed, rb.linearVelocity.y);

        if (moveHorizontal < 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveHorizontal > 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpTimer = jumpTime;
            rb.linearVelocity = Vector2.up * jumpForce;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimer > 0)
            {
                rb.linearVelocity = Vector2.up * jumpForce;
                jumpTimer -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }
}