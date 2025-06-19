using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DottopusBoss : MonoBehaviour
{
    [Header("Laser Attack")]
    public GameObject laserPrefab;
    public Transform[] laserSpawnPoints;
    public float laserSpeed = 8f;
    public float laserCooldown = 3f;
    public bool laserRandomSpawn = false;
    public int laserRandomCount = 4;
    public AudioClip laserSound;

    [Header("Orb Attack")]
    public GameObject orbPrefab;
    public Transform[] orbSpawnPoints;
    public float orbSpeed = 4f;
    public float orbCooldown = 6f;
    public bool orbRandomSpawn = false;
    public int orbRandomCount = 4;
    public AudioClip orbSound;

    [Header("Audio Sources")]
    public AudioSource laserAudioSource;
    public AudioSource orbAudioSource;
    public AudioSource aoeAudioSource;

    [Header("AoE Attack")]
    public GameObject aoePrefab;
    public Transform[] aoeSpawnPoints;
    public float aoeSpeed = 3f;
    public float aoeCooldown = 8f;
    public bool aoeRandomSpawn = false;
    public int aoeRandomCount = 4;
    public AudioClip aoeSound;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Damage Flash")]
    private SpriteRenderer[] spriteRenderers;
    private bool isTakingDamage = false;

    [Header("Animations")]
    public Animator allBodyAnimator;
    public Animator attackFaceAnimator;

    [Header("Portal Settings")]
    public GameObject portalPrefab; // Portal prefab
    public Transform portalSpawnPoint;

    public AudioClip deathSound;

    [Header("Detection")]
    public float detectionRange = 5f; // Oyuncu algılama mesafesi
    private Transform player;  // Oyuncu referansı

    private AudioSource audioSource;

    private float lastLaserAttackTime;
    private float lastOrbAttackTime;
    private float lastAoEAttackTime;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        player = GameObject.FindGameObjectWithTag("Player").transform; // Oyuncu referansını al
        StartCoroutine(FireLaserRoutine());
        StartCoroutine(FireOrbRoutine());
        StartCoroutine(FireAoERoutine());
    }

    void Update()
    {
        // Oyuncunun boss'a olan mesafesini kontrol et
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Eğer oyuncu detectionRange içinde ise ateş et
        if (distanceToPlayer <= detectionRange)
        {
            if (Time.time >= lastLaserAttackTime + laserCooldown)
            {
                TriggerAttackFace();
                FireProjectiles(laserPrefab, laserSpawnPoints, laserSpeed, laserSound, laserAudioSource, laserRandomSpawn, laserRandomCount);
                lastLaserAttackTime = Time.time;
            }

            if (Time.time >= lastOrbAttackTime + orbCooldown)
            {
                TriggerAttackFace();
                FireProjectiles(orbPrefab, orbSpawnPoints, orbSpeed, orbSound, orbAudioSource, orbRandomSpawn, orbRandomCount);
                lastOrbAttackTime = Time.time;
            }

            if (Time.time >= lastAoEAttackTime + aoeCooldown)
            {
                TriggerAttackFace();
                FireProjectiles(aoePrefab, aoeSpawnPoints, aoeSpeed, aoeSound, aoeAudioSource, aoeRandomSpawn, aoeRandomCount);
                lastAoEAttackTime = Time.time;
            }
        }
    }

    IEnumerator FireLaserRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(laserCooldown);
            FireProjectiles(laserPrefab, laserSpawnPoints, laserSpeed, laserSound, laserAudioSource, laserRandomSpawn, laserRandomCount);
        }
    }

    IEnumerator FireOrbRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(orbCooldown);
            FireProjectiles(orbPrefab, orbSpawnPoints, orbSpeed, orbSound, orbAudioSource, orbRandomSpawn, orbRandomCount);
        }
    }

    IEnumerator FireAoERoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(aoeCooldown);
            FireProjectiles(aoePrefab, aoeSpawnPoints, aoeSpeed, aoeSound, aoeAudioSource, aoeRandomSpawn, aoeRandomCount);
        }
    }

    void FireProjectiles(GameObject prefab, Transform[] spawnPoints, float speed, AudioClip sound, AudioSource source, bool isRandom, int randomCount)
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position); // Oyuncu ile mesafeyi hesapla

        if (distanceToPlayer <= detectionRange) // Eğer oyuncu detectionRange içinde ise
        {
            if (isRandom)
            {
                List<int> usedIndexes = new List<int>();
                int count = Mathf.Min(randomCount, spawnPoints.Length);
                for (int i = 0; i < count; i++)
                {
                    int index;
                    do { index = Random.Range(0, spawnPoints.Length); }
                    while (usedIndexes.Contains(index));
                    usedIndexes.Add(index);

                    FireSingle(prefab, spawnPoints[index], speed);
                }
            }
            else
            {
                foreach (Transform point in spawnPoints)
                {
                    FireSingle(prefab, point, speed);
                }
            }

            // Ses çalma
            if (sound != null && source != null)
            {
                Debug.Log("Çalan ses: " + sound.name);  // Test: Ses ismi yazacak
                source.PlayOneShot(sound);  // Ses çal
            }
            else
            {
                Debug.LogWarning("Ses kaynağı veya ses dosyası bulunamadı!");
            }
        }
    }

    void FireSingle(GameObject prefab, Transform spawnPoint, float speed)
    {
        GameObject proj = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.linearVelocity = spawnPoint.right * speed;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (!isTakingDamage)
            StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator DamageFlash()
    {
        isTakingDamage = true;

        foreach (var sr in spriteRenderers)
            sr.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        foreach (var sr in spriteRenderers)
            sr.color = Color.white;

        isTakingDamage = false;
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
        Destroy(gameObject);
        GameObject healthBarObject = GameObject.Find("DottopusHealthBar");
        if (healthBarObject != null)
        {
            healthBarObject.SetActive(false);
        }
    }

    void TriggerAttackFace()
    {
        if (attackFaceAnimator != null)
        {
            attackFaceAnimator.SetTrigger("Attack");
        }
    }
}
