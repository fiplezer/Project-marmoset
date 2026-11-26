using UnityEngine;
using UnityEngine.InputSystem;

public class Speler_Controller : MonoBehaviour
{
    //toggles turn on/off certain features
    [Header("Toggles")]
    public bool MovementToggle = true;
    public bool FreeMovementToggle = true;
    public bool JumpToggle = true;
    [Header("Move")]
    [SerializeField] private float moveSpeed = 5.5f;
    [Header("Jump")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float coyoteTime = 0.1f; //allows jumping when airborne
    [SerializeField] private float jumpBufferTime = 0.1f; //allows jumping when landing
    //[SerializeField] private float jumpCutMultiplier = 1f; //allows variable height
    [Header("GroundCheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask groundLayer;


    [Header("Components")]
    private Rigidbody2D rb;
    private InputAction moveInput;
    private InputAction freeMoveInput;
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
        freeMoveInput = InputSystem.actions.FindAction("FreeMove");
        jumpInput = InputSystem.actions.FindAction("Jump");
        //jump input, FindAction("Jump");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void JumpCheck()
    {
        if (jumpInput.WasPressedThisFrame())
        {
            jumpBufferCounter = jumpBufferTime;
        }
    }

    private void Jump()
    {
        JumpCheck();
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

        //jump if buffered input & in coyoteTime
        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
        }
    }

    void Update()
    {
        if (JumpToggle)
        {
            Jump();
        }

        if (MovementToggle && !FreeMovementToggle)
        {
            rb.gravityScale = 2f;
            moveVector = moveInput.ReadValue<Vector2>();
        }
        else
        {
            rb.gravityScale = 0f;
            moveVector = freeMoveInput.ReadValue<Vector2>();
        }
    }

    private void FixedUpdate()
    {
        if (MovementToggle)
        {
            if (!FreeMovementToggle)
            {
                rb.linearVelocity = new Vector2(moveVector.x * moveSpeed, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(moveVector.x * moveSpeed, moveVector.y * moveSpeed);
            }
        }
    }

}