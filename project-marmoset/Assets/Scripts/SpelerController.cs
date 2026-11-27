using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

interface IInteractable
{
    public void Interact();
}

public class SpelerController : MonoBehaviour
{
    //toggles turn on/off certain features
    [Header("Toggles")]
    [Tooltip("Besluit of de speler kan bewegen of niet.")]
    public bool MovementToggle = true;
    [Tooltip("Besluit of de speler elke kant op kan bewegen of alleen op de x-as.")]
    public bool FreeMovementToggle = true;
    [Tooltip("Besluit of de speler kan springen.")]
    public bool JumpToggle = true;
    [Tooltip("Besluit of de speler kan bukken.")]
    public bool CrouchToggle = true;
    [Tooltip("Besluit of de speler kan interacten/schieten")]
    public bool InteractToggle = false;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5.5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 10f;

    [Tooltip("Hoeveelheid tijd dat de speler in de lucht kan zijn en nog de mogelijkheid heeft om te springen.")]
    [SerializeField] private float coyoteTime = 0.1f; //allows jumping when airborne

    [Tooltip("Hoeveelheid tijd dat de speler de spring input kan bufferen voordat ze landen.")]
    [SerializeField] private float jumpBufferTime = 0.1f; //allows jumping when landing
    //[SerializeField] private float jumpCutMultiplier = 1f; //allows variable height

    [Header("GroundCheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask groundLayer;

    [Header("Interact")]
    [SerializeField] private float interactRange;
    [SerializeField] private LayerMask interactionMask;


    [Header("Components")]
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Vector2 moveVector;
    private bool isGrounded;
    private float coyoteCounter;
    private float jumpBufferCounter;

    [Header("InputActions")]
    private InputAction moveInput;
    private InputAction freeMoveInput;
    private InputAction jumpInput;
    private InputAction crouchInput;
    private InputAction interactInput;

    [Header("Coroutine")]
    private Coroutine unCrouch;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        moveInput = InputSystem.actions.FindAction("Move");
        freeMoveInput = InputSystem.actions.FindAction("FreeMove");
        jumpInput = InputSystem.actions.FindAction("Jump");
        crouchInput = InputSystem.actions.FindAction("Crouch");
        interactInput = InputSystem.actions.FindAction("Interact");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputValue value)
    {
        if (MovementToggle && !FreeMovementToggle)
        {
            rb.gravityScale = 2f;
            moveVector = value.Get<Vector2>();
            
            rb.linearVelocity = new Vector2(moveVector.x * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.gravityScale = 2f;
        }
    }

    public void OnFreeMove(InputValue value)
    {
        if (MovementToggle && FreeMovementToggle)
        {
            rb.gravityScale = 0f;
            moveVector = value.Get<Vector2>();

            rb.linearVelocity = new Vector2(moveVector.x * moveSpeed, moveVector.y * moveSpeed);

        }
        else
        {
            rb.gravityScale = 2f;
        }
    }

    public void OnJump(InputValue value)
    {
        if (JumpToggle)
        {
            if (value.Get<float>() >= 0.5f)
            {
                jumpBufferCounter = jumpBufferTime;
            }

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
    }

    public void OnCrouch(InputValue value)
    {
        if (CrouchToggle) 
        {
            if (value.Get<float>() >= 0.5f)
            {
                boxCollider.size = new Vector2(1f, 1f);
            }

            if (unCrouch != null)
            {
                StopCoroutine(unCrouch);
            }
            unCrouch = StartCoroutine(Uncrouch());
        }
    }

    public void OnInteract(InputValue value)
    {
        if (InteractToggle)
        {
            if (value.Get<float>() == 1f)
            {
                var col = Physics2D.OverlapCircleAll(transform.position, interactRange, interactionMask);

                if (col != null)
                {
                    foreach (var c in col)
                    {
                        if (c.TryGetComponent(out IInteractable interactable))
                        {
                            interactable.Interact();
                        }
                    }
                }
            }
        }
    }

    IEnumerator Uncrouch()
    {
        yield return new WaitForSeconds(1f);

        boxCollider.size = new Vector2(1f, 2f);
    }
}