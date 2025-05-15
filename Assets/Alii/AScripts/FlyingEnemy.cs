using UnityEngine;
using System.Collections;

public class FlyingEnemy : MonoBehaviour
{
    public int maxHealth = 50;
    public float speed = 3f;
    public float attackSpeed = 1f;
    public int attackDamage = 10;
    public float attackRange = 1.5f;
    public float detectionRange = 5f;
    public float randomMovementFactor = 0.5f;
    public float flashDuration = 0.2f;
    public bool alwaysChase = false;

    private Transform player;
    private int currentHealth;
    private Vector2 randomDirection;
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
        originalColor = spriteRenderer.color;
        randomDirection = Random.insideUnitCircle.normalized;

        // Animator bileşenini al
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (!playerDetected && distanceToPlayer <= detectionRange)
        {
            playerDetected = true;
        }

        if (playerDetected && (alwaysChase || distanceToPlayer <= detectionRange))
        {
            ChasePlayer(distanceToPlayer);

            if (distanceToPlayer <= attackRange && Time.time >= timeSinceLastAttack + attackSpeed)
            {
                AttackPlayer();
                timeSinceLastAttack = Time.time;
            }
            else
            {
                // Saldırı yapmıyorsa uçma animasyonuna dön
                animator.SetBool("isAttacking", false);
            }
        }
        else if (!alwaysChase)
        {
            playerDetected = false;
            // Uzaklaşınca uçma animasyonuna dön
            animator.SetBool("isAttacking", false);
        }
    }

    void ChasePlayer(float distance)
    {
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 randomOffset = randomDirection * randomMovementFactor;
        Vector2 finalDirection = (direction + randomOffset).normalized;

        Vector2 newPosition = (Vector2)transform.position + finalDirection * speed * Time.deltaTime;
        GetComponent<Rigidbody2D>().MovePosition(newPosition);

        if (Random.value < 0.05f)
        {
            randomDirection = Random.insideUnitCircle.normalized;
        }
    }

    void AttackPlayer()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log("FlyingEnemy saldırdı! Hasar: " + attackDamage);
        }

        // Saldırı animasyonunu tetikle
        animator.SetBool("isAttacking", true);
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
        Debug.Log("FlyingEnemy öldü!");
        Destroy(gameObject);
    }
}
