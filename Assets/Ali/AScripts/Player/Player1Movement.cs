using UnityEngine;
using UnityEngine.InputSystem;

public class Player1Movement : MonoBehaviour
{
    private Vector2 moveInput;
    private bool stopPressed = false;

    public float speed = 150f;
    public float jumpingPower = 280f;
    public float variableJumpMultiplier = 0.5f;
    private bool isJumping = false;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Animator lowerBodyAnimator;
    [SerializeField] private GameObject legs;
    [SerializeField] private SpriteRenderer upperBodyRenderer;

    private PlayerInput playerInput;

    private void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Stop"].performed += ctx => stopPressed = true;
        playerInput.actions["Stop"].canceled += ctx => stopPressed = false;

        playerInput.actions["Jump"].performed += OnJumpPressed;
        playerInput.actions["Jump"].canceled += OnJumpReleased;
    }

    private void OnDisable()
    {
        playerInput.actions["Stop"].performed -= ctx => stopPressed = true;
        playerInput.actions["Stop"].canceled -= ctx => stopPressed = false;

        playerInput.actions["Jump"].performed -= OnJumpPressed;
        playerInput.actions["Jump"].canceled -= OnJumpReleased;
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnJumpPressed(InputAction.CallbackContext ctx)
    {
        if (IsGrounded())
        {
            isJumping = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }
    }

    private void OnJumpReleased(InputAction.CallbackContext ctx)
    {
        if (isJumping && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * variableJumpMultiplier);
        }
        isJumping = false;
    }

    void Start()
    {
        lowerBodyAnimator = legs.GetComponent<Animator>();
    }

    void Update()
    {
        // Alt vücut animasyonu
        if (stopPressed)
        {
            lowerBodyAnimator.SetFloat("Speed", 0f);
        }
        else
        {
            lowerBodyAnimator.SetFloat("Speed", Mathf.Abs(moveInput.x));
        }

        // Yön çevirme
        if (moveInput.x < 0f && isFacingRight)
        {
            Flip();
        }
        else if (moveInput.x > 0f && !isFacingRight)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        float moveSpeed = stopPressed ? 0f : moveInput.x * speed;
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 characterScale = transform.localScale;
        characterScale.x *= -1f;
        transform.localScale = characterScale;

        if (upperBodyRenderer != null)
        {
            Vector3 upperScale = upperBodyRenderer.transform.localScale;
            upperScale.x *= -1f;
            upperBodyRenderer.transform.localScale = upperScale;
        }
    }
}
