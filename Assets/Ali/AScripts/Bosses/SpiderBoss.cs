using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpiderBoss : MonoBehaviour
{
    [Header("Combat Settings")]
    public int maxHealth = 200;
    public float detectionRange = 10f;
    public float attackCooldown = 2f;
    public GameObject bulletPrefab;
    public Transform[] bulletSpawnPoints;
    public int attackDamage = 15;
    public float bulletSpeed = 8f;
    public float fireIntervalBetweenHands = 0.3f;
    [Header("Audio Sources")]
public AudioSource attackAudioSource;  // Ateş sesi
public AudioSource teleportAudioSource;  // Teleport sesi
public AudioSource deathAudioSource;  // Ölüm sesi

    public AudioClip attackSound;
    public AudioClip deathSound;
    public AudioClip teleportSound;
    public float flashDuration = 0.2f;
    public Animator animator;
    public GameObject teleportVFX;

    [Header("Portal Settings")]
    public GameObject portalPrefab; // Portal prefabı
    public Transform portalSpawnPoint; // Portal nereye çıkacak

    [Header("Teleport Settings")]
    public Transform[] teleportPoints;
    public float teleportMinTime = 8f;
    public float teleportMaxTime = 15f;

    public int currentHealth;
    private float lastAttackTime;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private AudioSource audioSource;
    private Transform targetPlayer;
    private Vector3 originalScale;
    private bool isTeleporting = false;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        originalScale = transform.localScale;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        StartCoroutine(TeleportRoutine());
        
    }

    void Update()
    {
        if (isTeleporting) return;

        targetPlayer = GetNearestPlayer();
        if (targetPlayer == null)
        {
            SetAttackState(false);
            return;
        }

        float distance = Vector2.Distance(transform.position, targetPlayer.position);

        // Range içinde mi kontrolü
        bool inRange = distance <= detectionRange;
        SetAttackState(inRange);

        if (!inRange) return;

        if (Time.time >= lastAttackTime + attackCooldown && IsInAttackState())
        {
            FaceTarget(targetPlayer.position);
            StartCoroutine(ShootBothHands());
            lastAttackTime = Time.time;

            if (attackSound)
                audioSource.PlayOneShot(attackSound);
        }
    }

    IEnumerator ShootBothHands()
    {
        foreach (Transform point in bulletSpawnPoints)
        {
            Shoot(targetPlayer.position, point);
            yield return new WaitForSeconds(fireIntervalBetweenHands);
            if (attackSound && attackAudioSource)
    attackAudioSource.PlayOneShot(attackSound);  // Ateş sesi çal
        }
    }

    void Shoot(Vector3 targetPos, Transform spawnPoint)
    {
        Vector2 direction = (targetPos - spawnPoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = direction * bulletSpeed;

        Bullet b = bullet.GetComponent<Bullet>();
        if (b != null)
            b.damage = attackDamage;
    }

    IEnumerator TeleportRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(teleportMinTime, teleportMaxTime);
            yield return new WaitForSeconds(waitTime);
            yield return StartCoroutine(Teleport());
        }
    }

    IEnumerator Teleport()
    {
        isTeleporting = true;

        // Ateşi durdur, idle’a geçsin
        SetAttackState(false);

        if (animator) animator.SetTrigger("TeleportOut");
        if (teleportVFX) Instantiate(teleportVFX, transform.position, Quaternion.identity);
        if (teleportSound) audioSource.PlayOneShot(teleportSound);

        yield return new WaitForSeconds(1f);

        Transform newPos = teleportPoints[Random.Range(0, teleportPoints.Length)];
        transform.position = newPos.position;

        if (teleportVFX) Instantiate(teleportVFX, transform.position, Quaternion.identity);
        if (animator) animator.SetTrigger("TeleportIn");
        if (teleportSound && teleportAudioSource)
    teleportAudioSource.PlayOneShot(teleportSound);  // Teleport sesi çal

        yield return new WaitForSeconds(1f);

        isTeleporting = false;
    }

    void SetAttackState(bool isAttacking)
    {
        if (animator != null)
            animator.SetBool("isAttacking", isAttacking);
    }

    bool IsInAttackState()
    {
        if (animator == null) return false;
        return animator.GetBool("isAttacking");
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
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        else
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
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
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    void Die()
    {
        if (deathSound) audioSource.PlayOneShot(deathSound);

        // PORTAL SPAWN
        if (portalPrefab != null && portalSpawnPoint != null)
        {
            Instantiate(portalPrefab, portalSpawnPoint.position, Quaternion.identity);
            Debug.Log("Portal spawnlandı."); // TEST için
        }
        else
        {
            Debug.LogWarning("Portal prefab veya spawn point eksik!");
        }
        if (deathSound && deathAudioSource)
    deathAudioSource.PlayOneShot(deathSound);  // Ölüm sesi çal

        Destroy(gameObject);
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
}