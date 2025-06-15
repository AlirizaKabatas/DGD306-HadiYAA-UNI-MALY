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

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        StartCoroutine(FireLaserRoutine());
        StartCoroutine(FireOrbRoutine());
        StartCoroutine(FireAoERoutine());
    }

    IEnumerator FireLaserRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(laserCooldown);
            TriggerAttackFace();
            FireProjectiles(laserPrefab, laserSpawnPoints, laserSpeed, laserSound, laserRandomSpawn, laserRandomCount);
        }
    }

    IEnumerator FireOrbRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(orbCooldown);
            TriggerAttackFace();
            FireProjectiles(orbPrefab, orbSpawnPoints, orbSpeed, orbSound, orbRandomSpawn, orbRandomCount);
        }
    }

    IEnumerator FireAoERoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(aoeCooldown);
            TriggerAttackFace();
            FireProjectiles(aoePrefab, aoeSpawnPoints, aoeSpeed, aoeSound, aoeRandomSpawn, aoeRandomCount);
        }
    }

    void FireProjectiles(GameObject prefab, Transform[] spawnPoints, float speed, AudioClip sound, bool isRandom, int randomCount)
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

        if (sound && audioSource) audioSource.PlayOneShot(sound);
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
