using UnityEngine;
using System.Collections;

public class SlimeEnemy : MonoBehaviour
{
    [Header("Combat")]
    public int maxHealth = 50;
    public float speed = 3f;
    public float attackSpeed = 1f;
    public int attackDamage = 10;
    public float attackRange = 1.5f;
    public float detectionRange = 5f;
    public float flashDuration = 0.2f;
    public bool alwaysChase = false;
    public float postAttackDelay = 0.5f; // 🆕 Saldırı sonrası bekleme süresi

    [Header("Audio")]
    public AudioSource idleAudio;
    public AudioSource attackAudio;
    public AudioSource deathAudio;

    private int currentHealth;
    private float timeSinceLastAttack;
    private float lastAttackEndTime;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Animator animator;
    private Transform targetPlayer;

    private float fixedY; // 🆕 Y pozisyonunu sabitlemek için

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        if (idleAudio != null)
            idleAudio.Play();

        fixedY = transform.position.y; // 🆕 Başlangıç Y konumu
    }

    void FixedUpdate()
    {
        targetPlayer = GetClosestPlayer();
        if (targetPlayer == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, targetPlayer.position);

        if (distanceToPlayer <= detectionRange)
        {
            FacePlayer();

            if (Time.time >= lastAttackEndTime + postAttackDelay)
            {
                animator.SetBool("isChasing", true);
                animator.SetBool("isAttacking", false);

                if (alwaysChase || distanceToPlayer > attackRange)
                {
                    ChasePlayer();
                }

                if (distanceToPlayer <= attackRange && Time.time >= timeSinceLastAttack + attackSpeed)
                {
                    AttackPlayer();
                    timeSinceLastAttack = Time.time;
                    lastAttackEndTime = Time.time;
                }
            }
        }
        else
        {
            animator.SetBool("isChasing", false);
            animator.SetBool("isAttacking", false);
        }
    }

    void FacePlayer()
    {
        if (targetPlayer != null)
        {
            Vector3 scale = transform.localScale;
            scale.x = targetPlayer.position.x < transform.position.x ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (targetPlayer.position - transform.position).normalized;

        // Sadece X yönünde hareket et, Y sabit kalsın
        Vector3 newPos = new Vector3(transform.position.x + direction.x * speed * Time.fixedDeltaTime, fixedY, transform.position.z);
        transform.position = newPos;
    }

    void AttackPlayer()
    {
        if (attackAudio != null) attackAudio.Play();

        PlayerHealth playerHealth = targetPlayer.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }

        animator.SetBool("isAttacking", true);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
            Die();
    }

    IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
        }
    }

    void Die()
    {
        if (deathAudio != null)
            deathAudio.Play();

        Destroy(gameObject, 0.1f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
                TakeDamage(bullet.damage);

            Destroy(other.gameObject);
        }
    }

    Transform GetClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject p in players)
        {
            float dist = Vector2.Distance(transform.position, p.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = p.transform;
            }
        }

        return closest;
    }
}
