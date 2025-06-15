using UnityEngine;

public class Player1Movement : MonoBehaviour
{
    private float horizontal;
    public float speed = 150f;
    public float jumpingPower = 280f;
    private bool isFacingRight = true;
    private bool isSpeedZero = false; // C tuşu ile hızı sıfırlamak için

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Animator lowerBodyAnimator;       // Bacak animasyonu
    [SerializeField] private GameObject legs;                  // Legs GameObject
    [SerializeField] private SpriteRenderer upperBodyRenderer; // Üst gövde sprite'ı

    void Start()
    {
        lowerBodyAnimator = legs.GetComponent<Animator>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // C tuşuna basılıysa hız 0 olacak
        isSpeedZero = Input.GetKey(KeyCode.C);

        // Alt vücut animasyonu
        if (isSpeedZero)
        {
            lowerBodyAnimator.SetFloat("Speed", 0f); // Animasyonu durdur
        }
        else
        {
            lowerBodyAnimator.SetFloat("Speed", Mathf.Abs(horizontal)); // Hareket animasyonu
        }

        // Yön kontrolü
        if (horizontal < 0f && isFacingRight)
        {
            Flip();
        }
        else if (horizontal > 0f && !isFacingRight)
        {
            Flip();
        }

        // Zıplama
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }

        // Zıplamayı erken bırakma
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
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

        // Ana karakter objesini ters çevir
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

        // Legs'e dokunma (parent olduğu için otomatik döner)
    }
}
