using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Movement : MonoBehaviour
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

        // 🔁 Hareket bağlama
        playerInput.actions["Move2"].performed += ctx => OnMove(ctx);
        playerInput.actions["Move2"].canceled += ctx => OnMove(ctx);

        // 🛑 Stop tuşu
        playerInput.actions["Stop2"].performed += ctx => stopPressed = true;
        playerInput.actions["Stop2"].canceled += ctx => stopPressed = false;

        // 🦘 Zıplama
        playerInput.actions["Jump2"].performed += OnJumpPressed;
        playerInput.actions["Jump2"].canceled += OnJumpReleased;
    }

    private void OnDisable()
    {
        playerInput.actions["Move2"].performed -= ctx => OnMove(ctx);
        playerInput.actions["Move2"].canceled -= ctx => OnMove(ctx);

        playerInput.actions["Stop2"].performed -= ctx => stopPressed = true;
        playerInput.actions["Stop2"].canceled -= ctx => stopPressed = false;

        playerInput.actions["Jump2"].performed -= OnJumpPressed;
        playerInput.actions["Jump2"].canceled -= OnJumpReleased;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
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
        // Animasyon
        lowerBodyAnimator.SetFloat("Speed", stopPressed ? 0f : Mathf.Abs(moveInput.x));

        // Yön değiştirme
        if (moveInput.x < 0f && isFacingRight) Flip();
        else if (moveInput.x > 0f && !isFacingRight) Flip();
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

        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;

        if (upperBodyRenderer != null)
        {
            Vector3 upperScale = upperBodyRenderer.transform.localScale;
            upperScale.x *= -1f;
            upperBodyRenderer.transform.localScale = upperScale;
        }
    }
}
