using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    private float horizontal;
    public float speed = 150f;
    public float jumpingPower = 280f;
    private bool isFacingRight = true;
    private bool isSpeedZero = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Animator lowerBodyAnimator;
    [SerializeField] private GameObject legs;
    [SerializeField] private SpriteRenderer upperBodyRenderer;

    void Start()
    {
        lowerBodyAnimator = legs.GetComponent<Animator>();
    }

    void Update()
    {
        // Yatay hareket tuşları: Sağ ve Sol ok tuşları
        if (Input.GetKey(KeyCode.LeftArrow)) horizontal = -1f;
        else if (Input.GetKey(KeyCode.RightArrow)) horizontal = 1f;
        else horizontal = 0f;

        // L tuşuna basılıysa hız sıfırlanacak
        isSpeedZero = Input.GetKey(KeyCode.L);

        // Alt vücut animasyonu
        if (isSpeedZero)
        {
            lowerBodyAnimator.SetFloat("Speed", 0f);
        }
        else
        {
            lowerBodyAnimator.SetFloat("Speed", Mathf.Abs(horizontal));
        }

        // Yön çevirme
        if (horizontal < 0f && isFacingRight)
        {
            Flip();
        }
        else if (horizontal > 0f && !isFacingRight)
        {
            Flip();
        }

        // Zıplama: . tuşu
        if (Input.GetKeyDown(KeyCode.Period) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }

        // Zıplamayı erken bırakma
        if (Input.GetKeyUp(KeyCode.Period) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    void FixedUpdate()
    {
        float moveSpeed = isSpeedZero ? 0f : horizontal * speed;
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        // Karakteri çevir
        Vector3 characterScale = transform.localScale;
        characterScale.x *= -1f;
        transform.localScale = characterScale;

        // Üst vücut sprite'ını da çevir
        if (upperBodyRenderer != null)
        {
            Vector3 upperScale = upperBodyRenderer.transform.localScale;
            upperScale.x *= -1f;
            upperBodyRenderer.transform.localScale = upperScale;
        }
    }
}
