using UnityEngine;
using UnityEngine.InputSystem;

public class Speler_Controller : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 5.5f;
    [Header("Jump")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float coyoteTime = 0.1f; //allows jumping when airborne
    [SerializeField] private float jumpBufferTime = 0.1f; //allows jumping when landing
    [SerializeField] private float jumpCutMultiplier = 1f; //allows variable height
    [Header("GroundCheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask groundLayer;


    [Header("Components")]
    private Rigidbody2D rb;
    private InputAction moveInput;
    private Vector2 moveVector;
    private InputAction jumpInput;
    private bool isGrounded;
    private float coyoteCounter;
    private float jumpBufferCounter;
    private bool jumpHeld;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveInput = InputSystem.actions.FindAction("Move");
        jumpInput = InputSystem.actions.FindAction("Jump");
        //jump input, FindAction("Jump");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnJump()
    {
        if (jumpInput.ReadValue<float>() == 1f)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        jumpHeld = true;
        if (jumpInput.ReadValue<float>() == 0f)
        {
            jumpHeld = false;
        }
    }

    void Update()
    {
        OnJump();
        //check if player is grounded
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
        }    
        else
        {
            coyoteCounter -= Time.deltaTime;
        }
        jumpBufferCounter -= Time.deltaTime;

        //Debug.Log(coyoteCounter);
        //Debug.Log(jumpBufferCounter);

        //jump if buffered input & in coyoteTime
        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
        }

        //Variable jump height
        if (!jumpHeld && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }

        moveVector = moveInput.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveVector.x * moveSpeed, rb.linearVelocity.y);
    }

}