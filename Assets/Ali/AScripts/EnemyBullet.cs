using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 15;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Otomatik yok olma
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
