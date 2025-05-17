using UnityEngine;
using System.Collections;

public class L1Enemy : MonoBehaviour
{
    public int maxHealth = 50;
    public float speed = 3f;
    public float attackSpeed = 1f;
    public int attackDamage = 10;
    public float attackRange = 1.5f;
    public float detectionRange = 5f;
    public float flashDuration = 0.2f;
    public bool alwaysChase = false;

    private Transform player;
    private int currentHealth;
    private float timeSinceLastAttack;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool playerDetected = false;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogError("SpriteRenderer bulunamadı!");
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Oyuncu menzile girince tespit et
        if (!playerDetected && distanceToPlayer <= detectionRange)
        {
            playerDetected = true;
            animator.SetBool("isChasing", true);
            animator.SetBool("isAttacking", false);
        }

        // Takip ve saldırı davranışı
        if (playerDetected && (alwaysChase || distanceToPlayer <= detectionRange))
        {
            ChasePlayer(distanceToPlayer);

            // Saldırı mesafesindeyse
            if (distanceToPlayer <= attackRange && Time.time >= timeSinceLastAttack + attackSpeed)
            {
                AttackPlayer();
                timeSinceLastAttack = Time.time;
            }
            else
            {
                animator.SetBool("isAttacking", false);
            }
        }
        else
        {
            // Oyuncu menzil dışına çıktıysa
            playerDetected = false;
            animator.SetBool("isChasing", false);
            animator.SetBool("isAttacking", false);
        }
    }

    void ChasePlayer(float distance)
    {
        // Oyuncuya doğru hareket et
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 newPosition = (Vector2)transform.position + direction * speed * Time.deltaTime;
        transform.position = newPosition;

        // Hareket ederken yürüyüş animasyonunu başlat
        animator.SetBool("isChasing", true);
    }

    void AttackPlayer()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log("L1Enemy saldırdı! Hasar: " + attackDamage);
        }

        // Saldırı animasyonunu tetikle
        animator.SetBool("isAttacking", true);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("L1Enemy hasar aldı: " + damage);

        StartCoroutine(FlashRed());

        // Can sıfır veya altına düştüyse öl
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            Debug.Log("Kırmızıya dönme çalıştı.");
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
        }
    }

    void Die()
    {
        Debug.Log("L1Enemy öldü!");
        // Ölme animasyonunu oynat (isteğe bağlı)
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Mermi çarpışması kontrolü
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("L1Enemy mermi tarafından vuruldu.");
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
            }

            Destroy(other.gameObject);
        }
    }
}
