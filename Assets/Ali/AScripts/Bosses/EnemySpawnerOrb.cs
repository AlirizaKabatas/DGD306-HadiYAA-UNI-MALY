using UnityEngine;

public class EnemySpawnerOrb : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public LayerMask groundLayer;

    [Header("Visual & Sound Effects")]
    public GameObject hitVFX;
    public GameObject breakVFX;
    public AudioClip hitSound;
    public AudioClip breakSound;

    [Header("Settings")]
    public string playerBulletTag = "Bullet";

    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player mermisi çarptıysa
        if (collision.CompareTag(playerBulletTag))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
        // Ground layer ile temas
        else if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            SpawnEnemy();
            Destroy(gameObject);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Vurulma efekti
        if (hitVFX != null)
            Instantiate(hitVFX, transform.position, Quaternion.identity);

        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);

        if (currentHealth <= 0)
        {
            Break();
        }
    }

    void Break()
    {
        // Kırılma efekti ve sesi
        if (breakVFX != null)
            Instantiate(breakVFX, transform.position, Quaternion.identity);

        if (breakSound != null)
            audioSource.PlayOneShot(breakSound);

        Destroy(gameObject);
    }

    void SpawnEnemy()
    {
        if (enemyPrefab)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
    }
}
