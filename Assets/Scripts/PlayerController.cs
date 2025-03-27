using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck; 
    public LayerMask groundLayer; 
    public GameObject[] platforms; // Birden fazla platform için array
    private Rigidbody2D rb;
    private bool isGrounded;
    private PlatformCollider[] platformScripts; // PlatformCollider referansları

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        platformScripts = new PlatformCollider[platforms.Length];

        // Tüm platformlara script referansları al
        for (int i = 0; i < platforms.Length; i++)
        {
            platformScripts[i] = platforms[i].GetComponent<PlatformCollider>();
        }
    }

    void Update()
    {
        // Hareket
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y); // linearVelocity yerine velocity kullan

        // Sprite'ı hareket yönüne göre döndürme
        if (moveInput > 0) // Sağ hareket
        {
            // Yalnızca X eksenini tersine çevir
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        else if (moveInput < 0) // Sol hareket
        {
            // Yalnızca X eksenini tersine çevir
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }

        // Zemin Kontrolü
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Zıplama
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Aşağı inme (Platform içinden geçme)
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && isGrounded)
        {
            StartCoroutine(DisableCollisions());
        }
    }

    // Platformların çarpışmalarını geçici olarak devre dışı bırakma
    private IEnumerator DisableCollisions()
    {
        foreach (var platformScript in platformScripts)
        {
            platformScript.EnableCollider(false);  // Platformun çarpışmasını geçici olarak kapat
        }

        yield return new WaitForSeconds(0.4f);  // 0.4 saniye bekle

        foreach (var platformScript in platformScripts)
        {
            platformScript.EnableCollider(true);  // Çarpışmayı tekrar aç
        }
    }
}
