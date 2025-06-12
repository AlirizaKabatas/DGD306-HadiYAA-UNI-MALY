using UnityEngine;

public class Movement2 : MonoBehaviour
{
    private float horizontal;
    public float speed = 150f;
    public float jumpingPower = 280f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Animator lowerBodyAnimator;       // Bacak animasyonu
    [SerializeField] private GameObject legs;                  // Legs GameObject
    [SerializeField] private SpriteRenderer upperBodyRenderer; // Üst gövde sprite'ı

    void Start()
    {
        // Legs objesinin Animator component'ini al
        lowerBodyAnimator = legs.GetComponent<Animator>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");  // Yatay giriş

        // Alt vücut animasyonu
        lowerBodyAnimator.SetFloat("Speed", Mathf.Abs(horizontal));

        // Yön kontrolü (yalnızca basılı tutma veya bırakmaya göre yönlenir)
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
        // Karakteri yatay eksende hareket ettir
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        // Ana karakter objesini ters çevir (bu legs'i de döndürür)
        Vector3 characterScale = transform.localScale;
        characterScale.x *= -1f;
        transform.localScale = characterScale;

        // Üst vücut bağımsızsa, onu da çevir (örn: silah tutuyorsa falan)
        if (upperBodyRenderer != null)
        {
            Vector3 upperScale = upperBodyRenderer.transform.localScale;
            upperScale.x *= -1f;
            upperBodyRenderer.transform.localScale = upperScale;
        }

        // ❌ legs.transform.localScale'e DOKUNMA!
        // çünkü legs zaten parent (karakter) ile birlikte döner.
    }
}
