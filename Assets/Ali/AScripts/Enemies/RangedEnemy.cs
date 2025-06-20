using UnityEngine;
using System.Collections;

public class RangedEnemy : MonoBehaviour
{
    [Header("Combat Settings")]
    public int maxHealth = 30;
    public float detectionRange = 5f;
    public float attackCooldown = 2f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public int attackDamage = 10;
    public float bulletSpeed = 7f;
    public AudioClip attackSound;
    public AudioClip deathSound;
    public float flashDuration = 0.2f;
    public Animator animator;
    


    [Header("Shooting Settings")]
    [Range(0f, 90f)]
    public float fireAngle = 45f; // sadece sağa-sola 45 derece ateş

    private int currentHealth;
    private float lastAttackTime;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    [SerializeField] private AudioSource audioSource;
    private Transform targetPlayer;
    private Vector3 originalScale;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        originalScale = transform.localScale;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        targetPlayer = GetNearestPlayer();
        if (targetPlayer == null) return;

        float distance = Vector2.Distance(transform.position, targetPlayer.position);
        if (distance <= detectionRange && Time.time >= lastAttackTime + attackCooldown)
        {
            FaceTarget(targetPlayer.position);
            Shoot(targetPlayer.position);
            lastAttackTime = Time.time;

            if (attackSound)
                audioSource.PlayOneShot(attackSound);
        }
    }

    Transform GetNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject p in players)
        {
            float dist = Vector2.Distance(transform.position, p.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = p.transform;
            }
        }

        return nearest;
    }

    void FaceTarget(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        if (dir.x < 0)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
    }

   void Shoot(Vector3 targetPos)
{
    if (bulletPrefab == null || bulletSpawnPoint == null) return;

    if (animator != null)
    {
        animator.SetBool("isAttacking", true);
    }

    Vector2 direction = (targetPos - bulletSpawnPoint.position).normalized;

    // sadece yatay (x yönünde) 45 derece açıyla atış yap (yukarı aşağı yok)
    float angle = Vector2.Angle(Vector2.right, direction);
    if (angle > fireAngle && angle < 180 - fireAngle)
    {
        if (animator != null)
            animator.SetBool("isAttacking", false); // atış iptal edildi, bool kapat
        return;
    }

    GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        rb.linearVelocity = direction * bulletSpeed;
    }

    float angleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    bullet.transform.rotation = Quaternion.Euler(0, 0, angleZ);

    if (direction.x > 0)
    {
        Vector3 scale = bullet.transform.localScale;
        scale.x *= -1;
        bullet.transform.localScale = scale;
    }

    Bullet b = bullet.GetComponent<Bullet>();
    if (b != null)
    {
        b.damage = attackDamage;
    }

    // isAttacking'i kısa süre sonra kapat
    Invoke(nameof(ResetAttack), 0.2f);
}
    void ResetAttack()
{
    if (animator != null)
    {
        animator.SetBool("isAttacking", false);
    }
}


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    void Die()
{
    if (deathSound && audioSource)
    {
        audioSource.PlayOneShot(deathSound);
        Destroy(gameObject, deathSound.length); // 🎧 Ses kadar bekle sonra yok et
    }
    else
    {
        Destroy(gameObject);
    }
}


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
            }

            Destroy(other.gameObject);
        }
    }
}
