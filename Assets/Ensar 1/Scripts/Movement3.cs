using UnityEngine;

public class Movement3 : MonoBehaviour
{
    private float horizontal;
    public float speed = 30f;
    public float jumpingPower = 60f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Update()
    {
        // Yön kontrolü
        if (Input.GetKey(KeyCode.L))
        {
            horizontal = 1f;
        }
        else if (Input.GetKey(KeyCode.J))
        {
            horizontal = -1f;
        }
        else
        {
            horizontal = 0f;
        }

        // Zıplama
        if (Input.GetKeyDown(KeyCode.M) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }

        // Zıplama kısa kesme (buton erken bırakılırsa)
        if (Input.GetKeyUp(KeyCode.M) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
